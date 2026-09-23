using BusCima.Models;
using Microsoft.EntityFrameworkCore;

namespace BusCima.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Resenia> Resenias { get; set; }
        public DbSet<Parada> Paradas { get; set; }
    }
}