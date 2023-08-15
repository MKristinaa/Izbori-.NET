using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class LoginDto
    {
        public string? Tip { get; set; }
        [Required]
        public string? KorisnickoIme { get; set; }
        [Required]
        public string? Token { get; set; }
    }
}
