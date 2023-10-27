using System.IO.Compression;
using WebGLives.BusinessLogic.Services.Abstract;

namespace WebGLives.BusinessLogic.Services;

public class ZipService : IZipService
{
    public bool IsValid(string filePath) => 
        true;

    public void Extract(string sourceFilePath, string destinationFilePath) => 
        ZipFile.ExtractToDirectory(sourceFilePath, destinationFilePath);
}