namespace WebGLives.Core;

public class FileData : IDisposable, IAsyncDisposable
{
    public readonly string Name;
    public readonly string Extension;
    public readonly Stream Stream;

    public FileData(string name, string extension, Stream stream)
    {
        Name = name;
        Extension = extension;
        Stream = stream;
    }
    
    public void Dispose() =>
        Stream.Dispose();

    public async ValueTask DisposeAsync() =>
        await Stream.DisposeAsync();
}