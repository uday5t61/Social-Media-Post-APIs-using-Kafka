﻿using Confluent.Kafka;
using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Cmd.Infrastructure.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers
{
    public class EventSourcingHandler(IEventStore eventStore,IEventProducer producer) : IEventSourcingHandler<PostAggregate>
    {

        public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
        {
            var aggregate = new PostAggregate();

            var events = await eventStore.GetEventsAsync(aggregateId);

            if (events == null || events.Count == 0) return aggregate;

            aggregate.ReplayEvents(events);
            aggregate.Version = events.Select(x => x.Version).Max();

            return aggregate;
        }

        public async Task RepublishEventsAsync()
        {

            var aggregateIds = await eventStore.GetAggregateIdsAsync();

            if (aggregateIds == null || aggregateIds.Count == 0) return;

            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");

            foreach (var aggregateId in aggregateIds)
            {
                var aggregate = await GetByIdAsync(aggregateId);
                if (aggregate == null || !aggregate.Active) continue;

                var events = await eventStore.GetEventsAsync(aggregateId);

                foreach (var @event in events)
                {
                    await producer.ProduceAsync(topic, @event);
                }
            }
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await eventStore.SaveEventAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}
