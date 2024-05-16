using Lobby.Api.Models.Lobby;
using Lobby.Application.Lobbyist.Commands.CreateLobby;
using Lobby.Application.Lobbyist.Commands.UpdateLobby;
using Lobby.Application.Lobbyist.Queries.GetLobby;
using Lobby.Application.Lobbyist.Queries.GetLobbyist;

namespace Lobby.Api.Apis.Lobbies;

public static class LobbyApi
{
    public static IEndpointRouteBuilder MapLobbyApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/createLobby", CreateLobby);

        app.MapPost("/updateLobby", UpdateLobby);
        
        app.MapGet("/getLobbyist", GetLobbyist);

        app.MapGet("/getLobby/{id:guid}", GetLobby);
        
        return app;
    }

    private static async Task<IResult> CreateLobby([AsParameters] LobbyService lobbyService, CreateLobbyRequest request)
    {
        var command = lobbyService.Mapper.Map<CreateLobbyCommand>(request);

        var result = await lobbyService.Mediator.Send(command);

        return TypedResults.Ok(result);
    }
    
    private static async Task<IResult> UpdateLobby([AsParameters] LobbyService lobbyService, UpdateLobbyRequest request)
    {
        var command = lobbyService.Mapper.Map<UpdateLobbyCommand>(request);

        var result = await lobbyService.Mediator.Send(command);
        
        return TypedResults.Ok();
    }
    
    private static async Task<IResult> GetLobbyist([AsParameters] LobbyService lobbyService, [AsParameters] GetLobbyistRequest request)
    {
        var query = lobbyService.Mapper.Map<GetLobbyistQuery>(request);

        var result = await lobbyService.Mediator.Send(query);

        return TypedResults.Ok(result);
    }
    
    private static async Task<IResult> GetLobby([AsParameters] LobbyService lobbyService, Guid id)
    {
        var command = lobbyService.Mapper.Map<GetLobbyQuery>(id);

        var lobbyResult = await lobbyService.Mediator.Send(command);
        
        // TODO Error
        
        return TypedResults.Ok(lobbyResult.Value);
    }
}