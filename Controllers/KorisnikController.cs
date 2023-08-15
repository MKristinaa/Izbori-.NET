using Backend.Dtos;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Controllers
{
    [Route("/")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;
        private readonly DataContext dc;

        public KorisnikController(IUnitOfWork uow, IConfiguration configuration, DataContext dc)
        {
            this.uow = uow;
            this.configuration = configuration;
            this.dc = dc;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(login2 log)
        {
            string token = null;

            DtoUserJwt dtoUser = new DtoUserJwt();

            var admin = await dc.Admini.FirstOrDefaultAsync(x => x.KorisnickoIme == log.KorisnickoIme);
            var stranka = await dc.Stranke!.FirstOrDefaultAsync(x => x.KorisnickoIme == log.KorisnickoIme);
            var korisnik = await dc.Korisnici!.FirstOrDefaultAsync(x => x.KorisnickoIme == log.KorisnickoIme);

            if (admin != null && MatchPasswordHash(log.Lozinka, admin.Lozinka, admin.LozinkaKljuc))
            {
                dtoUser.KorisnickoIme = admin.KorisnickoIme;
                dtoUser.Id = admin.Id;
                dtoUser.Tip = "Admin";
                token = CreateJWT(dtoUser);

                var loginResponse = CreateLoginResponse(admin.Tip, admin.KorisnickoIme, token);
                return Ok(loginResponse);
            }
            else if (stranka != null && MatchPasswordHash(log.Lozinka, stranka.Lozinka, stranka.LozinkaKljuc))
            {
                dtoUser.KorisnickoIme = stranka.KorisnickoIme;
                dtoUser.Id = stranka.Id;
                dtoUser.Tip = "Stranka";
                token = CreateJWT(dtoUser);

                var loginResponse = CreateLoginResponse(stranka.Tip, stranka.KorisnickoIme, token);
                return Ok(loginResponse);
            }
            else if (korisnik != null && MatchPasswordHash(log.Lozinka, korisnik.Lozinka, korisnik.LozinkaKljuc))
            {
                dtoUser.KorisnickoIme = korisnik.KorisnickoIme;
                dtoUser.Id = korisnik.Id;
                dtoUser.Tip = "Korisnik";
                token = CreateJWT(dtoUser);

                var loginResponse = CreateLoginResponse(korisnik.Tip, korisnik.KorisnickoIme, token);
                return Ok(loginResponse);
            }
            else
            {
                return Unauthorized("Invalid credentials");
            }
        }

        private LoginDto CreateLoginResponse(string tip, string korisnickoIme, string token)
        {
            return new LoginDto
            {
                Tip = tip,
                KorisnickoIme = korisnickoIme,
                Token = token
            };
        }

        private string CreateJWT(DtoUserJwt user)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

            var claims = new Claim[]
            {
            new Claim(ClaimTypes.Name, user.KorisnickoIme),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Tip)
            };

            var signingCredentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(100),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordText));
                return CryptographicOperations.FixedTimeEquals(passwordHash, password);
            }
        }


        [HttpPost("/user_register")]
        public async Task<IActionResult> UserRegister(RegistarDto dto)
        {
            return await RegisterUser(dto);
        }

        [HttpPost("/admin_register")]
        public async Task<IActionResult> AdminRegister(RegistarDto dto)
        {
            return await RegisterUser(dto);
        }

        [HttpPost("/party_register")]
        public async Task<IActionResult> PartyRegister(RegistarDto dto)
        {
            return await RegisterUser(dto);
        }

        private async Task<IActionResult> RegisterUser(RegistarDto loginReq)
        {
            if (await uow.KorisnikRepository.UserAlreadyExists(loginReq.KorisnickoIme!))
                return BadRequest("User vec postoji");

            uow.KorisnikRepository.Register(loginReq);
            await uow.SaveAsync();
            return Ok(loginReq);
        }

        [HttpGet("GetStranke")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await uow.KorisnikRepository.GetAllStranke();
            return Ok(result);
        }

        [HttpGet("dohvatiStranku/{id}")]
        public IActionResult DohvatiStranku(int id)
        {
            var stranka = dc.Stranke.FirstOrDefault(k => k.Id == id);

            if (stranka == null)
            {
                return NotFound();
            }

            return Ok(stranka);
        }

        [HttpGet("dohvatiKorisnika/{id}")]
        public IActionResult DohvatiKorisnika(int id)
        {
            var korisnik = dc.Korisnici.FirstOrDefault(k => k.Id == id);

            if (korisnik == null)
            {
                return NotFound();
            }

            return Ok(korisnik);
        }


        [HttpPost("proveriUclanjenje")]
        public IActionResult ProveriUclanjenje([FromBody] Pripada pripada)
        {
            bool korisnikJeUclanjen = dc.Pripada.Any(p => p.IdKorisnika == pripada.IdKorisnika);

            if (korisnikJeUclanjen)
            {
                return Ok(new { jesteClan = true, message = "Već ste član neke stranke!" });
            }
            else
            {
                return Ok(new { jesteClan = false });
            }
        }


        [HttpPost("uclaniKorisnika")]
        public IActionResult UclaniKorisnika([FromBody] Pripada pripada)
        {
            try
            {
                // Provera da li korisnik već nije član stranke
                var vecJeClan = dc.Pripada.Any(p => p.IdKorisnika == pripada.IdKorisnika && p.IdStranke == pripada.IdStranke);
                if (vecJeClan)
                {
                    return BadRequest(new { message = "Već ste član neke stranke!" });
                }

                // Kreiranje novog učlanjenja
                var novoUclanjenje = new Pripada
                {
                    IdKorisnika = pripada.IdKorisnika,
                    IdStranke = pripada.IdStranke
                };

                dc.Pripada.Add(novoUclanjenje);
                dc.SaveChanges();

                return Ok(new { message = "Uspešno ste se učlanili!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Došlo je do greške pri učlanjenju.", error = ex.Message, stackTrace = ex.StackTrace });
            }
        }



        [HttpPost("proveriUclanjenje/{idKorisnika}")]
        public IActionResult ProveriUclanjenje(int idKorisnika)
        {
            // Proverite da li korisnik (idKorisnika) već pripada nekoj stranci
            bool korisnikJeUclanjen = dc.Pripada.Any(p => p.IdKorisnika == idKorisnika);

            if (korisnikJeUclanjen)
            {
                var strankaId = dc.Pripada.FirstOrDefault(p => p.IdKorisnika == idKorisnika)?.IdStranke;
                var stranka = dc.Stranke.FirstOrDefault(s => s.Id == strankaId);

                if (stranka != null)
                {
                    Console.WriteLine("Pronađena stranka:", stranka.Naziv);

                    return Ok(new { jesteClan = korisnikJeUclanjen, nazivStranke = stranka.Naziv, message = "Već ste član neke stranke!" });
                }
                else
                {
                    return Ok(new { jesteClan = korisnikJeUclanjen, message = "Nije moguće pronaći stranku za ovog korisnika." });
                }
            }
            else
            {
                return Ok(new { jesteClan = korisnikJeUclanjen, message = "Niste član nijedne stranke." });
            }
        }


        [HttpDelete("ukloniClanstvo/{idKorisnika}")]
        public IActionResult UkloniClanstvo(int idKorisnika)
        {
            // Pronađite članstvo korisnika
            var clanstvo = dc.Pripada.FirstOrDefault(p => p.IdKorisnika == idKorisnika);

            if (clanstvo != null)
            {
                // Ukloni članstvo korisnika iz stranke
                dc.Pripada.Remove(clanstvo);
                dc.SaveChanges();

                return Ok(new { message = "Članstvo je uspešno uklonjeno." });
            }
            else
            {
                return NotFound(new { message = "Korisnik nije član nijedne stranke." });
            }
        }



        [HttpGet("clanovi-stranke/{idStranke}")]
            public IActionResult GetClanoviStranke(int idStranke)
            {
                var clanoviStranke = dc.Pripada.Where(p => p.IdStranke == idStranke).Select(p => p.IdKorisnika).ToList();

                // Dohvatanje podataka o članovima iz tabele "Korisnici"
                var clanovi = dc.Korisnici.Where(k => clanoviStranke.Contains(k.Id)).ToList();

                return Ok(clanovi);
            }
    }
}
