using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebGLives.Core;
using WebGLives.Core.Repositories;
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
    
    public async Task<Game?> GetOrDefault(int id, CancellationToken token = default)
    {
        var game = await _context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);

        return game is null 
            ? null 
            : _mapper.Map<GameEntity, Game>(game);
    }
    
    public async Task<bool> Create(Game game, CancellationToken token = default)
    {
        var entity = _mapper.Map<Game, GameEntity>(game);
        await _context.Games.AddAsync(entity, token);
        var created = await _context.SaveChangesAsync(token);
        return created > 0;
    }

    public async Task<bool> Update(Game game, CancellationToken token = default)
    {
        var entity = _mapper.Map<Game, GameEntity>(game);
        _context.Games.Update(entity);
        var updated = await _context.SaveChangesAsync(token);
        return updated > 0;
    }

    public async Task<bool> Delete(int id, CancellationToken token = default)
    {
        var game = await _context.Games.FirstOrDefaultAsync(game => game.Id == id, token);

        if (game is null) 
            return false;

        _context.Games.Remove(game);
        var deleted = await _context.SaveChangesAsync(token);
        return deleted > 0;
    }
}