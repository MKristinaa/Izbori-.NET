using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Admin>? Admini { get; set; }
        public DbSet<Korisnik>? Korisnici { get; set; }
        public DbSet<Stranka>? Stranke { get; set; }
        public DbSet<Pripada>? Pripada { get; set; }
        public DbSet<Izbori>? Izbori { get; set; }
        public DbSet<Ucestvujee>? Ucestvujee { get; set; }
        public DbSet<Glas>? Glasovi { get; set; }


    }
}
