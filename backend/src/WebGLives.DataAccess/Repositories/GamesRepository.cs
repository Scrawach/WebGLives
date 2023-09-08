using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WebGLives.Core;
using WebGLives.Core.Errors;
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
    
    public async Task<Result<IEnumerable<Game>, Error>> All(CancellationToken token = default)
    {
        var games = await _context.Games
            .AsNoTracking()
            .ToArrayAsync(cancellationToken: token);

        return _mapper.Map<GameEntity[], Game[]>(games);
    }
    
    public async Task<Result<Game, Error>> GetOrDefault(int gameId, CancellationToken token = default)
    {
        var game = await _context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == gameId, cancellationToken: token);

        return game is not null
            ? _mapper.Map<GameEntity, Game>(game)
            : Result.Failure<Game, Error>(new GameNotFoundError(gameId));
    }
    
    public async Task<UnitResult<Error>> Create(Game game, CancellationToken token = default)
    {
        var entity = _mapper.Map<Game, GameEntity>(game);
        await _context.Games.AddAsync(entity, token);
        var created = await _context.SaveChangesAsync(token);
        return created > 0
            ? UnitResult.Success<Error>()
            : UnitResult.Failure<Error>(new Error($"Game not created!"));
    }

    public async Task<UnitResult<Error>> Update(Game game, CancellationToken token = default)
    {
        var entity = _mapper.Map<Game, GameEntity>(game);
        _context.Games.Update(entity);
        var updated = await _context.SaveChangesAsync(token);
        return updated > 0
            ? UnitResult.Success<Error>()
            : UnitResult.Failure<Error>(new Error($"Game not updated!"));
    }

    public async Task<UnitResult<Error>> Delete(int gameId, CancellationToken token = default)
    {
        var game = await _context.Games.FirstOrDefaultAsync(game => game.Id == gameId, token);

        if (game is null) 
            return UnitResult.Failure<Error>(new GameNotFoundError(gameId));

        _context.Games.Remove(game);
        var deleted = await _context.SaveChangesAsync(token);
        
        return deleted > 0
            ? UnitResult.Success<Error>()
            : UnitResult.Failure<Error>(new Error($"Game {gameId} not deleted!"));
    }
}