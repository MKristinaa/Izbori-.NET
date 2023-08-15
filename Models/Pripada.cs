using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Pripada
    {
        public int Id { get; set; }
        [ForeignKey("Korisnik")]
        public int IdKorisnika { get; set; }
        [ForeignKey("Stranka")]
        public int IdStranke { get; set; }
    }
}
