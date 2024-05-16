namespace Lobby.Domain;

public class GameLobby
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Guid> Players { get; set; } = new List<Guid>();
    public LobbyStatus Status { get; } = LobbyStatus.Test;
    public int MaxPlayers { get; set; }
    public int Points { get; set; }
    
    public DateTime CreatedDateTime { get; }
    public DateTime UpdatedDateTime { get; }

    public GameLobby(string name, int maxPlayers)
    {
        Id = Guid.NewGuid();
        Name = name;
        MaxPlayers = maxPlayers;
        Points = 100;
    }
}