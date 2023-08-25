using Microsoft.EntityFrameworkCore;
using WebGLives.DataAccess.Entities;

namespace WebGLives.DataAccess;

public class GamePageDbContext : DbContext
{
    public GamePageDbContext(DbContextOptions<GamePageDbContext> options) : base(options) { }
    
    public DbSet<GamePageEntity> GamePages { get; set; }
}