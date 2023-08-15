using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Dtos
{
    public class PripadaDto
    {
        public int IdKorisnika { get; set; }
        public int IdStranke { get; set; }
        public string NazivStranke { get; set; }
    }
}
