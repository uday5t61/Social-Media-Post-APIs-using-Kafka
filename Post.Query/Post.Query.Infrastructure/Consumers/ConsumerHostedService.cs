﻿using CQRS.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.Consumers
{
    public class ConsumerHostedService(ILogger<ConsumerHostedService> logger,IServiceProvider serviceProvider) : IHostedService
    {

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Event consumer service running");

            using IServiceScope scope = serviceProvider.CreateScope();
            var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");

            Task.Run(() => eventConsumer.Consume(topic), cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Event consumer service stopped");
            return Task.CompletedTask;
        }
    }
}
