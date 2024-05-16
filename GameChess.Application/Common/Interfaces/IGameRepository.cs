using GameChess.Domain.GameAggregate;
using GameChess.Domain.GameAggregate.ValueObjects;

namespace GameChess.Application.Common.Interfaces;

public interface IGameRepository
{
    Task<Game?> FindAsync(Guid id, CancellationToken cancellationToken = default);
    Task<long> AddAsync(Game aggregate, CancellationToken cancellationToken = default);
    Task<long> UpdateAsync(Game aggregate, long? expectedVersion = null, CancellationToken cancellationToken = default);
    Task<long> DeleteAsync(Game aggregate, long? expectedVersion = null, CancellationToken cancellationToken = default);
}