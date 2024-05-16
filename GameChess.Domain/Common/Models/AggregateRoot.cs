﻿namespace GameChess.Domain.Common.Models
{
    public abstract class AggregateRoot<TId> : Entity<TId>
        where TId : notnull
    {
        public int Version { get; }
        
        protected AggregateRoot(TId id)
        {
            Id = id;
        }

#pragma warning disable CS8618
        protected AggregateRoot()
        {
        }
#pragma warning restore CS8618
    }
}
