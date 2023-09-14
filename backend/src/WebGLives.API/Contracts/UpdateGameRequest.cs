namespace WebGLives.API.Contracts;

public class UpdateGameRequest
{
    public string? Title { get; set; }
    
    public IFormFile? Poster { get; set; }
    
    public string? Description { get; set; }
    
    public IFormFile? Game { get; set; }
}