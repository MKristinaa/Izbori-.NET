using Backend.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Stranka : RegisterDto
    {
        public int Id { get; set; }
        public string? Naziv { get; set; }
        public string? Slogan { get; set; }
        public string? NosilacListe { get; set;}
        public string? GodinaOsnivanja { get; set; }
        public string? Tip { get; set; }
        [Required]
        public string? KorisnickoIme { get; set; }
        [Required]
        public byte[]? Lozinka { get; set; }
        public byte[]? LozinkaKljuc { get; set; }
    }
}
