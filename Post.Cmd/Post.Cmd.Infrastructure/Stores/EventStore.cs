using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Post.Cmd.Infrastructure.Aggregates;

namespace Post.Cmd.Infrastructure.Stores
{
    public class EventStore(IEventStoreRepository eventStoreRepository) : IEventStore
    {
        public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            var eventStram = await eventStoreRepository.FindByAggrgateId(aggregateId);

            if (eventStram == null || eventStram.Count == 0)
            {
                throw new AggregateNotFoundException("Incorrect post Id provided!");
            }

            return eventStram.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
        }

        public async Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStram = await eventStoreRepository.FindByAggrgateId(aggregateId);

            if (expectedVersion != -1 && expectedVersion != eventStram[^1].Version)
            {
                throw new ConcurrencyException();
            }
            var version = expectedVersion;

            foreach (var @event in events)
            {
                version++;
                @event.Version = version;

                var eventType = @event.GetType().Name;
                var eventmodel = new Eventmodel
                {
                    AggregateIdentifier = aggregateId,
                    AggregateType = nameof(PostAggregate),
                    EventData = @event,
                    EventType = eventType,
                    TimeStamp = DateTime.Now,
                    Version = version
                };

                await eventStoreRepository.SaveAsync(eventmodel);
            }

        }
    }
}
