using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebGLives.Core;
using WebGLives.DataAccess.Entities;

namespace WebGLives.DataAccess.Repositories;

public class GamePageRepository : IGamePageRepository
{
    private readonly GamePageDbContext _context;
    private readonly IMapper _mapper;

    public GamePageRepository(GamePageDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public int Add(Game page)
    {
        var cardEntity = _mapper.Map<Game, GameEntity>(page);
        _context.GamePages.Add(cardEntity);
        _context.SaveChanges();
        return cardEntity.Id;
    }

    public IEnumerable<Game> All()
    {
        var pages = _context.GamePages
            .AsNoTracking()
            .ToArray();
        return _mapper.Map<GameEntity[], Game[]>(pages);
    }

    public Game GetById(int pageId)
    {
        var page = _context.GamePages
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == pageId);
        return _mapper.Map<GameEntity, Game>(page);
    }
}