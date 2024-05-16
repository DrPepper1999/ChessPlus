using GameChess.Api;
using GameChess.Api.Api.Games;
using GameChess.Application;
using GameChess.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("api/games")
    .WithTags("Games API")
    .MapGameApi();

app.Run();
