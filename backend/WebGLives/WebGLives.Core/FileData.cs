namespace WebGLives.Core;

public class FileData : IDisposable, IAsyncDisposable
{
    public readonly string Name;
    public readonly Stream Stream;

    public FileData(string name, Stream stream)
    {
        Name = name;
        Stream = stream;
    }

    public string GetExtension() =>
        Path.GetExtension(Name);

    public string GetNameWithoutExtension() =>
        Path.GetFileNameWithoutExtension(Name);

    public void Dispose() =>
        Stream.Dispose();

    public async ValueTask DisposeAsync() =>
        await Stream.DisposeAsync();
}