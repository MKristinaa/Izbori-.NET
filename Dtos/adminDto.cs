using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class adminDto
    {
        public string? Tip { get; set; }
        [Required]
        public string? KorisnickoIme { get; set; }
        [Required]
        public string? Lozinka { get; set; }
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
    }
}
