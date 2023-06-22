namespace WebGLives.API.Services;

public class GamePage
{
    public GamePage(string id, string title, string icon, string description)
    {
        Id = id;
        Title = title;
        Icon = icon;
        Description = description;
    }
    
    public string Id { get; }
    public string Title { get; }
    public string Icon { get; }
    public string Description { get; }
}