namespace Backend.Dtos
{
    public class izborDto
    {
        public string? Vrsta { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }
        public string? Grad { get; set; }
        public string? Otvoreni { get; set; }
    }
}
