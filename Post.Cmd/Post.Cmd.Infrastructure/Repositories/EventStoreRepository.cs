using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Config;

namespace Post.Cmd.Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<Eventmodel> _eventStoreCollections;

        public EventStoreRepository(IOptions<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);


            _eventStoreCollections = mongoDatabase.GetCollection<Eventmodel>(config.Value.Collection);
        }
        public async Task<List<Eventmodel>> FindByAggrgateId(Guid aggregateId)
        {
            return await _eventStoreCollections.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync().ConfigureAwait(false);
        }

        public async Task SaveAsync(Eventmodel @event)
        {
            await _eventStoreCollections.InsertOneAsync(@event).ConfigureAwait(false);
        }
    }
}
