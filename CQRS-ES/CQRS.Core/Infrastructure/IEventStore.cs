﻿using CQRS.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Infrastructure
{
    public interface IEventStore
    {
        Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
        Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);

        Task<List<Guid>> GetAggregateIdsAsync();
    }
}
