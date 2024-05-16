using Lobby.Api;
using Lobby.Api.Apis.Lobbies;
using Lobby.Api.Common.Mapping;
using Lobby.Application;
using Lobby.infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
}

var app = builder.Build();
{
    app.UseExceptionHandler();
    // app.UseInfrastructure();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    
    app.MapControllers();
    
    app.MapGroup("lobbyist")
        .WithTags("Lobbyist API")
        .MapLobbyApi();

    app.Run();
}