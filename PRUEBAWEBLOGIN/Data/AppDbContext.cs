using Microsoft.EntityFrameworkCore;
using PRUEBAWEBLOGIN.Models;

namespace PRUEBAWEBLOGIN.Data
{
    // contexto de base de datos  entity
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // tabla de usuarios
        public DbSet<User> Users { get; set; }
    }
}