using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class partyDto
    {
        public string? Tip { get; set; }
        [Required]
        public string? KorisnickoIme { get; set; }
        [Required]
        public string? Lozinka { get; set; }
        public string? Naziv { get; set; }
        public string? Slogan { get; set; }
        public string? NosilacListe { get; set; }
    }
}
