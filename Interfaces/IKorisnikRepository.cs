using Backend.Dtos;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IKorisnikRepository
    {
        Task<RegisterDto> Authenticate(string korisnickoIme, string lozinka);
        void Register(RegistarDto loginReq);
        Task<bool> UserAlreadyExists(string korisnickoIme);
        Task<IEnumerable<Stranka>> GetAllStranke();
    }
}
