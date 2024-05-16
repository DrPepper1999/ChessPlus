using MediatR;

namespace GameChess.Application.Games.Commands.CreateGame;

public record CreateGameCommand() : IRequest<Guid>;