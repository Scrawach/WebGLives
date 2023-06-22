namespace WebGLives.API.Services;

public class GamePage
{
    public GamePage(string id, string name, string icon, string description)
    {
        Id = id;
        Name = name;
        Icon = icon;
        Description = description;
    }
    
    public string Id { get; }
    public string Name { get; }
    public string Icon { get; }
    public string Description { get; }
}