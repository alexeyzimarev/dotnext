using System;
using System.Text.Json;
using EventSourcing.Infrastructure;
using EventStore.Client;

namespace DotNext.Infrastructure {
    public static class EventDeserializer {
        public static object Deserialize(this ResolvedEvent resolvedEvent) {
            var meta = JsonSerializer.Deserialize<EventMetadata>(resolvedEvent.Event.Metadata.Span);
            var dataType = Type.GetType(meta.ClrType);
            var data     = JsonSerializer.Deserialize(resolvedEvent.Event.Data.Span, dataType);
            return data;
        }
    }
}
