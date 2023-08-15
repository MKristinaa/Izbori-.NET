
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Dtos;

namespace Backend.Controllers
{
    [Route("/")]
    [ApiController]
    public class IzboriController : ControllerBase
    {
        private readonly DataContext dc;

        public IzboriController(DataContext dc)
        {
            this.dc = dc;
        }



        [HttpPost("noviIzbor")]
        public IActionResult DodajIzbor([FromBody] izborDto iz)
        {
            if (iz == null || !ModelState.IsValid)
            {
                return BadRequest("Nevažeći podaci za izbor.");
            }

            var noviIzbor = new Backend.Models.Izbori
            {
                Vrsta = iz.Vrsta,
                DatumPocetka = iz.DatumPocetka,
                DatumZavrsetka = iz.DatumZavrsetka,
                Grad = iz.Grad,
                Otvoreni = iz.Otvoreni

            };

            dc.Izbori!.Add(noviIzbor);
            dc.SaveChanges();

            return Ok(201);
        }

        [HttpGet("GetIzbore")]
        public IActionResult GetIzbori()
        {
            List<Izbori> izbori = dc.Izbori.ToList();
            return Ok(izbori);
        }

        [HttpGet("GetIzborById/{id}")]
        public IActionResult GetIzborById(int id)
        {
            Izbori izbor = dc.Izbori.FirstOrDefault(i => i.Id == id);
            if (izbor == null)
            {
                return NotFound(); 
            }

            return Ok(izbor);
        }

        [HttpGet("GetPrijavljeneStranke")]
        public IActionResult GetPrijavljeneStranke()
        {
            var ucestvuje = dc.Ucestvujee.ToList();
            return Ok(ucestvuje);
        }

        [HttpGet("prijavljivanjeStrankeByIdIzbora/{idIzbora}")]
        public IActionResult GetPrijavljeneStrankeByIdIzbora(int idIzbora)
        {
            var prijavljeneStranke = dc.Ucestvujee.Where(p => p.IdIzbora == idIzbora).Select(p => p.IdStranke).ToList();

            // Dohvatanje podataka o članovima iz tabele "Korisnici"
            var prijavljene = dc.Stranke.Where(k => prijavljeneStranke.Contains(k.Id)).ToList();

            return Ok(prijavljene);
        }

        [HttpPost("prijavljivanjeStranke")]
        public IActionResult ProveriUclanjenje([FromBody] Ucestvujee ucestvuje)
        {
            // Proverite da li postoji zapis za određenu stranku (ucestvuje.IdStranke) i određeni izbor (ucestvuje.IdIzbora)
            bool strankaJePrijavljena = dc.Ucestvujee.Any(p => p.IdStranke == ucestvuje.IdStranke && p.IdIzbora == ucestvuje.IdIzbora);

            if (strankaJePrijavljena)
            {
                return Ok(new { jesteClan = true, message = "Već ste prijavljeni na ovim izborima." });
            }
            else
            {
                // Stranka nije prijavljena za ovaj izbor, omogućite joj prijavu
                dc.Ucestvujee.Add(ucestvuje);
                dc.SaveChanges();

                return Ok(new { jesteClan = false, message = "Stranka je uspešno prijavljena za ovaj izbor." });
            }
        }

        [HttpPost("glasanje")]
        public IActionResult DavanjeGlasa([FromBody] Glas glas)
        {
            // Proverite da li korisnik (idKorisnika) već pripada nekoj stranci
            bool vecSteGlasali = dc.Glasovi.Any(p => p.IdKorisnika == glas.IdKorisnika && p.IdIzbora == glas.IdIzbora);

            if (vecSteGlasali)
            {
                return Ok(new { glas = vecSteGlasali, message = "Već ste glasali na ovim izborima!" });
            }
            else
            {
                // Korisnik nije učlanjen u nijednu stranku, omogućite mu učlanjenje u novu stranku
                dc.Glasovi.Add(glas);
                dc.SaveChanges();

                return Ok(new { glas = vecSteGlasali, message = "Uspešno ste glasali." });
            }
        }


        // Pobedniik
        // Glasovi 
        [HttpGet("pobednikIzbora/{idIzbora}")]
        public IActionResult GetWinnerOfElection(int idIzbora)
        {
            var winner = dc.Glasovi
                .Where(g => g.IdIzbora == idIzbora)
                .GroupBy(g => g.IdStranke)
                .Select(g => new
                {
                    IdStranke = g.Key,
                    BrojGlasova = g.Count()
                })
                .OrderByDescending(g => g.BrojGlasova)
                .FirstOrDefault();

            if (winner != null)
            {
                // Dohvatanje podataka o pobedničkoj stranci na osnovu IdStranke
                var pobednikStranke = dc.Stranke.FirstOrDefault(s => s.Id == winner.IdStranke);

                if (pobednikStranke != null)
                {
                    // Pravimo anonimni objekat koji sadrži informacije o pobedničkoj stranci i broju glasova
                    var pobednikInfo = new
                    {
                        Stranka = pobednikStranke,
                        BrojGlasova = winner.BrojGlasova
                    };

                    return Ok(pobednikInfo);
                }
                else
                {
                    return NotFound("Pobednička stranka nije pronađena."); // Stranka nije pronađena
                }
            }
            else
            {
                return NotFound("Nema glasova za ovaj izbor."); // Ako nema glasova za ovaj izbor, vratimo Not Found status
            }
        }

        [HttpPut("updateElectionStatus/{id}")]
        public IActionResult UpdateElectionStatus(int id, [FromBody] Status statusUpdate)
        {
            var existingElection = dc.Izbori.FirstOrDefault(e => e.Id == id);
            if (existingElection == null)
            {
                return NotFound("Izbor nije pronađen.");
            }

            // Ažuriranje statusa izbora
            existingElection.Otvoreni = statusUpdate.Otvoreni;

            // Sačuvajte izmene u bazi podataka
            dc.SaveChanges();

            return Ok("Uspješno ažuriran status izbora.");
        }

    }
}
