using CQRS_Core.Domain;
using CQRS_Core.Handlers;
using CQRS_Core.Infrastructure;
using Post.Cmd.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure.Handlers;

public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
{
    private readonly IEventStore _eventStore;

    public EventSourcingHandler(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task<PostAggregate> GetByIdAsync(Guid AggregateId)
    {
        var aggregate = new PostAggregate();
        var events = await _eventStore.GetEventsAsync(AggregateId);

        if(events == null || !events.Any())
        {
            return aggregate;
        }

        aggregate.ReplayEvents(events);
        aggregate.Version = events.Select(x => x.Version).Max();

        return aggregate;
    }

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await _eventStore.SaveEventAsync(aggregate.Id, aggregate.GetUncommitedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommited();
    }
}
