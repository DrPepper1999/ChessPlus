using FluentValidation;

namespace Lobby.Application.Lobbyist.Queries.GetLobbyist;

public class GetLobbyistQueryHandlerValidator : AbstractValidator<GetLobbyistQuery>
{
    public GetLobbyistQueryHandlerValidator()
    {
        RuleFor(q => q.PageSize).GreaterThan(0);
        RuleFor(q => q.PageNumber).GreaterThanOrEqualTo(0);
    }
}