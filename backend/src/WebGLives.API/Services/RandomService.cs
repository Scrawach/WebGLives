using System.Text;

namespace WebGLives.API.Services;

public class RandomService : IRandomService
{
    private readonly Random _random;

    public RandomService(int seed = 0) =>
        _random = new Random(seed);

    public string Word(int length)
    {
        var stringBuilder = new StringBuilder(length);
        for (var i = 0; i < length; i++) 
            stringBuilder.Append(Letter());
        return stringBuilder.ToString();
    }

    public char Letter()
    {
        const int asciiStart = 97;
        const int asciiEnd = 122;
        var randomAsciiCode = _random.Next(asciiStart, asciiEnd + 1);
        return (char)randomAsciiCode;
    }
}