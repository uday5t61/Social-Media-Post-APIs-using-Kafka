﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace CQRS.Core.Events
{
    public class Eventmodel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid AggregateIdentifier { get; set; }
        public required string AggregateType { get; set; }
        public int Version { get; set; }
        public required string EventType { get; set; }
        public required BaseEvent EventData { get; set; }
    }
}
