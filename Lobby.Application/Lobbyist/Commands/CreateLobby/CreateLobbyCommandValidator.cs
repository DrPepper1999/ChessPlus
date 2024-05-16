using FluentValidation;

namespace Lobby.Application.Lobbyist.Commands.CreateLobby;

public class CreateLobbyCommandValidator : AbstractValidator<CreateLobbyCommand>
{
    public CreateLobbyCommandValidator()
    {
        RuleFor(l => l.MaxPlayers).GreaterThan(2);
    }
}