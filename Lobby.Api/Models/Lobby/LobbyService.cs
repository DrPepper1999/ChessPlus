using MapsterMapper;
using MediatR;

namespace Lobby.Api.Models.Lobby;

public class LobbyService(ISender mediator, IMapper mapper)
{
    public ISender Mediator { get; } = mediator;
    public IMapper Mapper { get; } = mapper;
}