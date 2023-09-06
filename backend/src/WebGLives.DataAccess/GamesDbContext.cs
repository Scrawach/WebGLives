using Microsoft.EntityFrameworkCore;
using WebGLives.DataAccess.Configurations;
using WebGLives.DataAccess.Entities;

namespace WebGLives.DataAccess;

public class GamesDbContext : DbContext
{
    public GamesDbContext(DbContextOptions<GamesDbContext> options) : base(options) { }
    
    public DbSet<GameEntity> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new GameEntityConfiguration());
    }
}