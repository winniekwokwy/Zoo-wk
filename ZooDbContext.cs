using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

public class ZooDbContext : DbContext
{
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Classification> Classifications { get; set; }
    public DbSet<Species> AllSpecies { get; set; }
    protected readonly IConfiguration _Configuration;

    public ZooDbContext(IConfiguration configuration)
    {
        _Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sqlite database
        options.UseSqlite(_Configuration.GetConnectionString("WebApiDatabase"));
    }
}