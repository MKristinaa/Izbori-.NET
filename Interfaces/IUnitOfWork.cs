namespace Backend.Interfaces
{
    public interface IUnitOfWork
    {
        IKorisnikRepository KorisnikRepository { get; }
        Task<bool> SaveAsync();
    }
}
