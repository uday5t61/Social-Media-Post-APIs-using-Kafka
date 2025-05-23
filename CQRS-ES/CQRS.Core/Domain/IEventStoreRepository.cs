﻿using CQRS.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Domain
{
    public interface IEventStoreRepository
    {
        Task SaveAsync(Eventmodel @event);
        Task<List<Eventmodel>> FindByAggrgateId(Guid aggregateId);
        Task<List<Eventmodel>> FindAllAsync();
    }
}
