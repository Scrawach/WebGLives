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

    public int Add(GamePage page)
    {
        var cardEntity = _mapper.Map<GamePage, GamePageEntity>(page);
        _context.GamePages.Add(cardEntity);
        _context.SaveChanges();
        return cardEntity.Id;
    }

    public IEnumerable<GamePage> All()
    {
        var pages = _context.GamePages
            .AsNoTracking()
            .ToArray();
        return _mapper.Map<GamePageEntity[], GamePage[]>(pages);
    }

    public GamePage GetById(int pageId)
    {
        var page = _context.GamePages
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == pageId);
        return _mapper.Map<GamePageEntity, GamePage>(page);
    }
}