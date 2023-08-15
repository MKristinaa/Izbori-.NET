using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public abstract class RegisterDto
    {
        public int Id { get; set; }
        public string Tip { get; set; }
        public string KorisnickoIme { get; set; }
        public byte[] Lozinka { get; set; }
        public byte[] LozinkaKljuc { get; set; }
       
    }
}
