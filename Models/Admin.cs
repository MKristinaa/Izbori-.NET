using Backend.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Admin : RegisterDto
    {
        public int Id { get; set; }
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? Tip { get; set; }
        [Required]
        public string? KorisnickoIme { get; set; }
        [Required]
        public byte[]? Lozinka { get; set; }
        public byte[]? LozinkaKljuc { get; set; }
    }
}
