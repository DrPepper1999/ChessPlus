using GameChess.Api.Models;
using GameChess.Domain.GameAggregate;
using GameChess.Domain.GameAggregate.Enums;
using GameChess.Domain.GameAggregate.ValueObjects;

namespace GameChess.Api.Api.Games;

public static class GameApi
{
    public static IEndpointRouteBuilder MapGameApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/createGame", CreateGame);
        
        return app;
    }

    private static async Task<IResult> CreateGame([AsParameters] GameService service, CreateGameRequest request)
    {
        var game = Game.Create(new Dictionary<Color, PlayerId>() { { Color.White, PlayerId.CreateUnique() },
            { Color.Black, PlayerId.CreateUnique()} });

        var test2 = await service.LobbyService.GetLobbyAsync(request.LobbyId);
        
        await service.GameRepository.AddAsync(game);
        
        var test = await service.GameRepository.FindAsync(game.Id);
        
        return TypedResults.Ok(test);
    }
}