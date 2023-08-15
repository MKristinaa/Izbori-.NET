using Backend.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Korisnik : RegisterDto
    {
        public int Id { get; set; }
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? Email { get; set; }
        public DateTime? DatumRodjenja { get; set; }
        public string? Grad { get; set; }
        public string? BrojTelefona { get; set; }
        public string? Pol { get; set; }
        public string? Tip { get; set; }
        [Required]
        public string? KorisnickoIme { get; set; }
        [Required]
        public byte[]? Lozinka { get; set; }
        public byte[]? LozinkaKljuc { get; set; }
    }
}
