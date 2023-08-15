using Backend.Dtos;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Security.Cryptography;

namespace Backend.Repo
{
    public class KorisnikRepository : IKorisnikRepository
    {
        private readonly DataContext dc;

        public KorisnikRepository(DataContext dc)
        {
            this.dc = dc;
        }

        public async Task<RegisterDto> Authenticate(string korisnickoIme, string lozinka)
        {
            // Provera lozinke za korisnika
            var korisnik = await dc.Korisnici.FirstOrDefaultAsync(x => x.KorisnickoIme == korisnickoIme);
            if (korisnik != null && korisnik.Tip == "Korisnik" && MatchPasswordHash(lozinka, korisnik.Lozinka, korisnik.LozinkaKljuc))
                return korisnik;

            // Provera lozinke za admina
            var admin = await dc.Admini.FirstOrDefaultAsync(x => x.KorisnickoIme == korisnickoIme);
            if (admin != null && admin.Tip == "Admin" && MatchPasswordHash(lozinka, admin.Lozinka, admin.LozinkaKljuc))
                return admin;

            // Provera lozinke za stranku
            var stranka = await dc.Stranke.FirstOrDefaultAsync(x => x.KorisnickoIme == korisnickoIme);
            if (stranka != null && stranka.Tip == "Stranka" && MatchPasswordHash(lozinka, stranka.Lozinka, stranka.LozinkaKljuc))
                return stranka;

            // Ako korisnik sa unetim korisničkim imenom ne postoji ili je uneta pogrešna lozinka,
            // baci custom izuzetak sa odgovarajućom porukom.
            throw new Exception("Pogrešan tip korisnika ili neispravna lozinka.");
        }



        private bool MatchPasswordHash(string passwordText, byte[]? password, byte[]? passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));

                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != password[i])
                        return false;
                }

                return true;
            }
        }

        public void Register(RegistarDto loginReq)
        {
            if (string.IsNullOrEmpty(loginReq.Lozinka))
            {
                throw new ArgumentException("Lozinka nije unesena ili nije valjana.");
            }
            else
            {
                byte[] passwordHash, passwordKey;

                using (var hmac = new HMACSHA512())
                {
                    passwordKey = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginReq.Lozinka));
                }

                switch (loginReq.Tip.ToLower()) // Pretpostavka: tipKorisnika je u donjem slučaju (npr. "admin", "korisnik", "stranka")
                {
                    case "admin":
                        var admin = new Admin
                        {
                            Tip = loginReq.Tip,
                            KorisnickoIme = loginReq.KorisnickoIme,
                            Lozinka = passwordHash,
                            LozinkaKljuc = passwordKey,
                            Ime = loginReq.Ime,
                            Prezime = loginReq.Prezime,
                        };
                        dc.Admini.Add(admin);
                        break;
                    case "korisnik":
                        var korisnik = new Korisnik
                        {
                            Tip = loginReq.Tip,
                            KorisnickoIme = loginReq.KorisnickoIme,
                            Lozinka = passwordHash,
                            LozinkaKljuc = passwordKey,
                            Ime = loginReq.Ime,
                            Prezime = loginReq.Prezime,
                            Email = loginReq.Email,
                            Grad  = loginReq.Grad,
                            BrojTelefona = loginReq.BrojTelefona,
                            Pol = loginReq.Pol,
                            DatumRodjenja = loginReq.DatumRodjenja
                        };
                        dc.Korisnici.Add(korisnik);
                        break;
                    case "stranka":
                        var stranka = new Stranka
                        {
                            Tip = loginReq.Tip,
                            KorisnickoIme = loginReq.KorisnickoIme,
                            Lozinka = passwordHash,
                            LozinkaKljuc = passwordKey,
                            Naziv = loginReq.Naziv,
                            GodinaOsnivanja = loginReq.GodinaOsnivanja,
                            NosilacListe = loginReq.NosilacListe,
                            Slogan = loginReq.Slogan
                            
                        };
                        dc.Stranke.Add(stranka);
                        break;
                    default:
                        throw new ArgumentException("Nepoznat tip korisnika.");
                }
            }
        }

        public async Task<bool> UserAlreadyExists(string korisnickoIme)
        {
            // Proverite u tabelama Admin, Korisnik i Stranka da li postoji korisnik sa datim korisničkim imenom
            var adminExists = await dc.Admini.AnyAsync(x => x.KorisnickoIme == korisnickoIme);
            var korisnikExists = await dc.Korisnici.AnyAsync(x => x.KorisnickoIme == korisnickoIme);
            var strankaExists = await dc.Stranke.AnyAsync(x => x.KorisnickoIme == korisnickoIme);

            // Vratite true ako postoji korisnik u bilo kojoj od tabela, inače vratite false
            return adminExists || korisnikExists || strankaExists;
        }


        public async Task<IEnumerable<Stranka>> GetAllStranke()
        {

            return await dc.Stranke!.ToListAsync();
        }


    }
}
