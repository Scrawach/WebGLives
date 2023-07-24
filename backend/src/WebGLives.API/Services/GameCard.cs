namespace WebGLives.API.Services;

public class GameCard
{
    public GameCard(string id, string title, string icon, string description, string url)
    {
        Id = id;
        Title = title;
        Icon = icon;
        Description = description;
        Url = url;
    }
    
    public string Id { get; }
    public string Title { get; }
    public string Icon { get; }
    public string Description { get; }
    public string Url { get; }
}