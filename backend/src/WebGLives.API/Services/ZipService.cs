using System.IO.Compression;

namespace WebGLives.API.Services;

public class ZipService : IZipService
{
    public bool IsValid(string filePath) => 
        true;

    public void Extract(string sourceFilePath, string destinationFilePath) => 
        ZipFile.ExtractToDirectory(sourceFilePath, destinationFilePath);
}