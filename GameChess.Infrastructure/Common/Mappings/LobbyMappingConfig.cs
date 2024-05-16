using GameChess.Application.Models;
using GameChess.Infrastructure.LobbyService;
using Mapster;

namespace GameChess.Infrastructure.Common.Mappings;

public class LobbyMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LobbyResponse, Lobby>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Players, src =>
                src.Players.ToDictionary(k => k.Item1, v => v.Item2))
            .Map(dest => dest.Point, src => src.Point)
            .Map(dest => dest.MaxPlayers, src => src.MaxPlayers)
            .Map(dest => dest.Status, src => src.Status);
    }
}