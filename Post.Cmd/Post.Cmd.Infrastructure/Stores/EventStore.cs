using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Cmd.Infrastructure.Aggregates;

namespace Post.Cmd.Infrastructure.Stores
{
    public class EventStore(IEventStoreRepository eventStoreRepository,IEventProducer eventProducer) : IEventStore
    {
        public async Task<List<Guid>> GetAggregateIdsAsync()
        {
            var eventStram = await eventStoreRepository.FindAllAsync();
            if (eventStram == null || eventStram.Count == 0) throw new ArgumentNullException(nameof(eventStram), "Could not retrieve event stream from store");
            return [.. eventStram.Select(x => x.AggregateIdentifier).Distinct()];
        }

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

                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                await eventProducer.ProduceAsync(topic, @event);
            }

        }
    }
}
