using WebGLives.Auth.Identity;

namespace WebGLives.DataAccess.Entities;

public class GameEntity
{
    public int Id { get; set; }
    
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? PosterUrl { get; set; }
    
    public string? GameUrl { get; set; }
    
    public int UserId { get; set; }
    
    public User? User { get; set; }
}