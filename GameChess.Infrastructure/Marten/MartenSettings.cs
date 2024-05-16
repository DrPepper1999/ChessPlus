using Marten.Events.Daemon.Resiliency;

namespace GameChess.Infrastructure.Marten;

public class MartenSettings
{
    public const string SectionName = "EventStore";
    
    private const string DefaultSchema = "public";
    
    public string ConnectionString { get; set; } = null!;

    public string WriteModelSchema { get; set; } = DefaultSchema;
    public string ReadModelSchema { get; set; } = DefaultSchema;

    public bool ShouldRecreateDatabase { get; set; } = false;

    public DaemonMode DaemonMode { get; set; } = DaemonMode.Solo;

    public bool UseMetadata = true;
}