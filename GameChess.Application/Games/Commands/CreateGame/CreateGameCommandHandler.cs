using GameChess.Domain.GameAggregate;
using MediatR;

namespace GameChess.Application.Games.Commands.CreateGame;

public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, Guid>
{
    public Task<Guid> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}