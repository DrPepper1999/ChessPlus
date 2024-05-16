using Lobby.Application.Lobbyist.Queries.GetLobby;
using Mapster;

namespace Lobby.Api.Common.Mapping;

public class LobbyMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Guid, GetLobbyQuery>()
            .Map(dest => dest, src => new GetLobbyQuery(src));
    }
}