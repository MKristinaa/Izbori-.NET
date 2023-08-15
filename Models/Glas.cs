using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Glas
    {
        public int Id { get; set; }
        [ForeignKey("Korisnik")]
        public int IdKorisnika { get; set; }
        [ForeignKey("Izbori")]
        public int IdIzbora { get; set; }
        [ForeignKey("Stranka")]
        public int IdStranke { get; set; }
        public DateTime DatumGlasanja { get; set; }
        public string LokacijaGlasanja { get; set; }
    }
}
