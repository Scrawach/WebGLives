namespace WebGLives.API.Services;

public interface IZipService
{
    bool IsValid(string filePath);
    void Extract(string sourceFilePath, string destinationFilePath);
}