using GameChess.Domain.GameAggregate;
using GameChess.Domain.GameAggregate.Entities;
using GameChess.Domain.GameAggregate.Enums;
using GameChess.Domain.GameAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameChess.Infrastructure.Games.Persistence;

public class GameConfigurations : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");
        
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .ValueGeneratedNever();
            // .HasConversion(
            //     id => id,
            //     value => GameId.Create(value));

        ConfigureBoardTable(builder);
        
        builder.OwnsOne(g => g.Players, pb =>
        {
            pb.OwnsOne(typeof(Dictionary<Color, PlayerId>), "_players", ppb => { ppb.ToJson(); });

            pb.ToJson();
        });
    }

    private static void ConfigureBoardTable(EntityTypeBuilder<Game> builder)
    {
        builder.OwnsOne(typeof(Board), "_board", bb =>
        {
            bb.OwnsOne(typeof(Dictionary<Coordinates, Piece>), "_pieces", pb =>
            {
                pb.ToJson();
            });

            bb.ToJson();
        });
    }
    
}