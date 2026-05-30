using Microsoft.EntityFrameworkCore;

public class NewDbContext(DbContextOptions<NewDbContext> options) : DbContext(options)
{
    public DbSet<API.Models.Usuarios> Usuarios { get; set; } = default!;
}
