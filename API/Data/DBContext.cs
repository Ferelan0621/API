using Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Laboratorios> Laboratorios { get; set; }
        public DbSet<Prestamos> Prestamos { get; set; }
        public DbSet<Encargados> Encargados { get; set; }
    }
}
