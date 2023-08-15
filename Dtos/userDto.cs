using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class userDto
    {
        public string? Tip { get; set; }
        [Required]
        public string? KorisnickoIme { get; set; }
        [Required]
        public string? Lozinka { get; set; }
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? Email { get; set; }
        public string Grad { get; set; }
        public DateTime? DatumRodjenja { get; set; }
        public string? BrojTelefona { get; set; }
        public string? Pol { get; set; }
    }
}
