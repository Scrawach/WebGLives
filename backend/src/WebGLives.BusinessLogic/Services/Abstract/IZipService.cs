namespace WebGLives.BusinessLogic.Services.Abstract;

public interface IZipService
{
    bool IsValid(string filePath);
    void Extract(string sourceFilePath, string destinationFilePath);
}