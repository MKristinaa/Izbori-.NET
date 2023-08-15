using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("/")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly DataContext dc;

        public FilterController(DataContext dc)
        {
            this.dc = dc;
        }


        [HttpGet("clanovi-stranke/{idStranke}/filtered")]
        public IActionResult GetFilteredMembersClanoviStranke(int idStranke, [FromQuery] MemberFilterDto filterModel)
        {
            var clanoviStranke = dc.Pripada.Where(p => p.IdStranke == idStranke).Select(p => p.IdKorisnika).ToList();
            var clanovi = dc.Korisnici.Where(k => clanoviStranke.Contains(k.Id)).ToList();

            if (!string.IsNullOrEmpty(filterModel.ImePrezime))
                clanovi = clanovi
                    .Where(m => (m.Ime + " " + m.Prezime).Contains(filterModel.ImePrezime, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            if (filterModel.DatumRodjenja.HasValue)
                clanovi = clanovi.Where(m => m.DatumRodjenja == filterModel.DatumRodjenja.Value.Date).ToList();

            if (!string.IsNullOrEmpty(filterModel.Pol))
                clanovi = clanovi.Where(m => m.Pol.Contains(filterModel.Pol, StringComparison.OrdinalIgnoreCase)).ToList();

            return Ok(clanovi);
        }


        [HttpGet("rezultati/filtered")]
        public IActionResult FilterResults(string cityFilter)
        {
           var izbori = dc.Izbori.Where(election => election.Otvoreni == "Ne").ToList();

            if (!string.IsNullOrEmpty(cityFilter))
            {
                izbori = izbori.Where(election => election.Grad != null && election.Grad.ToLowerInvariant().Contains(cityFilter.ToLowerInvariant())).ToList();
            }



            return Ok(izbori);
        }


    }
}
