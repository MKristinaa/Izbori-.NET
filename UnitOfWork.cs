using Backend.Interfaces;
using Backend.Repo;

namespace Backend
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext dc;

        public UnitOfWork(DataContext dc)
        {
            this.dc = dc;
        }

        public IKorisnikRepository KorisnikRepository =>
                new KorisnikRepository(dc);
        public async Task<bool> SaveAsync()
        {
            return await dc.SaveChangesAsync() > 0;
        }
    }
}
