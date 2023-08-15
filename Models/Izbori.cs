namespace Backend.Models
{
    public class Izbori
    {
        public int Id { get; set; }
        public string? Vrsta { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }
        public string? Grad { get; set; }
        public string? Otvoreni { get; set; }
    }
}
