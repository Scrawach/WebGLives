namespace WebGLives.API.Services;

public class TempGamePageRepository : IGamePagesRepository
{
    private readonly List<GameCard> _cards = new List<GameCard>();
    
    public IEnumerable<GameCard> All() => 
        _cards;

    public void Create(GameCard card) => 
        _cards.Add(card);

    public GameCard GetById(string id) => 
        _cards.First(card => card.Id == id);
}