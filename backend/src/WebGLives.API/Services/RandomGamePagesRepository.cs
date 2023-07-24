namespace WebGLives.API.Services;

public class RandomGamePagesRepository : IGamePagesRepository
{
    private readonly IRandomService _random;

    public RandomGamePagesRepository(IRandomService random) =>
        _random = random;

    public IEnumerable<GameCard> All()
    {
        for (var i = 0; i < 25; i++)
            yield return RandomGamePage(i);
    }

    private GameCard RandomGamePage(int id) =>
        new(
            id.ToString(),
            _random.Word(10),
            "icon-url",
            _random.Word(25),
            "https://v6p9d9t4.ssl.hwcdn.net/html/7331557/cows-crasher-webgl/index.html"
        );
}