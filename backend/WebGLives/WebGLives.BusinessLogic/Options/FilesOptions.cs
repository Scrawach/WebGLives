namespace WebGLives.BusinessLogic.Options;

public class FilesOptions
{
    public const string Files = "Files";

    public string ProjectFolder { get; set; } = string.Empty;
    public string GamesFolder { get; set; } = string.Empty;
    public string BaseDirectory => Path.Combine(Path.GetTempPath(), ProjectFolder, GamesFolder);
}