using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Ucestvujee
    {
        public int Id { get; set; }
        [ForeignKey("Izbori")]
        public int IdIzbora { get; set; }
        [ForeignKey("Stranka")]
        public int IdStranke { get; set; }
    }
}
