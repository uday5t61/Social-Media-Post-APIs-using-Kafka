using Castle.Components.DictionaryAdapter.Xml;
using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.Consumers
{
    public class EventConsumer(IOptions<ConsumerConfig> config,IEventHandler eventHandler) : IEventConsumer
    {      
        public void Consume(string topic)
        {
            using var consumer = new ConsumerBuilder<string, string>(config.Value)
                 .SetKeyDeserializer(Deserializers.Utf8)
                 .SetValueDeserializer(Deserializers.Utf8)
                 .Build();

            consumer.Subscribe(topic);

            while (true)
            {
                var consumeResults = consumer.Consume();

                if (consumeResults?.Message == null) continue;

                var options = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new EventJsonConverters()
                    }
                };

                var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResults.Message.Value, options);

                var handlerMethod = eventHandler.GetType().GetMethod("On", [@event.GetType()]);

                if (handlerMethod == null) throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method! ");

                handlerMethod.Invoke(eventHandler, [@event]);
                consumer.Commit(consumeResults);
            }
        }
    }
}
