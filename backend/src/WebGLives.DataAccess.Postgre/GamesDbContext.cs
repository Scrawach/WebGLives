using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebGLives.Auth.Identity;
using WebGLives.DataAccess.Configurations;
using WebGLives.DataAccess.Entities;

namespace WebGLives.DataAccess;

public class GamesDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public GamesDbContext(DbContextOptions<GamesDbContext> options) : base(options) { }
    
    public DbSet<GameEntity> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new GameEntityConfiguration());
    }
}