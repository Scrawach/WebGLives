namespace WebGLives.API.Extensions;

public static class FormFileExtensions
{
    public static async Task CopyToAsync(this IFormFile file, string path)
    {
        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);
    }
}