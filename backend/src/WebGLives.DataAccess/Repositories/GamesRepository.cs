using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebGLives.Core;
using WebGLives.DataAccess.Entities;

namespace WebGLives.DataAccess.Repositories;

public class GamesRepository : IGamesRepository
{
    private readonly GamesDbContext _context;
    private readonly IMapper _mapper;

    public GamesRepository(GamesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<Game>> All(CancellationToken token = default)
    {
        var games = await _context.Games
            .AsNoTracking()
            .ToArrayAsync(cancellationToken: token);

        return _mapper.Map<GameEntity[], Game[]>(games);
    }
    
    public async Task<Game> Get(int id, CancellationToken token = default)
    {
        var game = await _context.Games
            .AsNoTracking()
            .FirstAsync(x => x.Id == id, cancellationToken: token);
        return _mapper.Map<GameEntity, Game>(game);
    }
    
    public async Task Add(Game game, CancellationToken token = default)
    {
        var entity = _mapper.Map<Game, GameEntity>(game);
        await _context.Games.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task Update(Game game, CancellationToken token = default)
    {
        var entity = _mapper.Map<Game, GameEntity>(game);
        _context.Games.Update(entity);
        await _context.SaveChangesAsync(token);
    }

    public async Task Delete(int id, CancellationToken token = default)
    {
        if (_context.Games.Any(game => game.Id == id))
        {
            _context.Remove(id);
            await _context.SaveChangesAsync(token);
        }
    }
}