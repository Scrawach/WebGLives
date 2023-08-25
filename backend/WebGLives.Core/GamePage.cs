namespace WebGLives.Core;

public class GamePage
{
    public GamePage(string id, string title, string description, string icon, string url)
    {
        Id = id;
        Title = title;
        Description = description;
        Icon = icon;
        Url = url;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public string Url { get; set; }
}