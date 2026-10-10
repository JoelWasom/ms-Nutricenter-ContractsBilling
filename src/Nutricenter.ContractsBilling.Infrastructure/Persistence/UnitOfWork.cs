using MediatR;
using Nutricenter.BuildingBlocks.Abstractions;
using System.Collections.Immutable;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence
{
    internal class UnitOfWork(DomainDbContext dbContext, IMediator mediator) : IUnitOfWork
    {
        private readonly DomainDbContext _dbContext = dbContext;
        private readonly IMediator _mediator = mediator;

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            var domainEvents = _dbContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents.Count != 0)
                .Select(entityEntry =>
                {
                    var domainEvents = entityEntry.Entity.DomainEvents.ToImmutableArray();
                    entityEntry.Entity.ClearDomainEvents();
                    return domainEvents;
                })
                .SelectMany(domainEvents => domainEvents)
                .ToList();
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
