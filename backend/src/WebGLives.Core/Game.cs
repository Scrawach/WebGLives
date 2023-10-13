namespace WebGLives.Core;

public class Game
{
    public const int MaxTitleLength = 10;
    public const int MaxDescriptionLength = 300;

    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? PosterUrl { get; set; }
    public string? GameUrl { get; set; }
    public int UserId { get; set; }
}