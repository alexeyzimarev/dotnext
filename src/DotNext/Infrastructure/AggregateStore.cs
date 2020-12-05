using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DotNext.Lib;
using EventSourcing.Infrastructure;
using EventStore.Client;

namespace DotNext.Infrastructure {
    public class AggregateStore : IAggregateStore {
        readonly EventStoreClient _client;

        public AggregateStore(EventStoreClient client) => _client = client;

        public async Task Store<T>(T aggregate) where T : Aggregate {
            if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));

            var stream  = GetStreamName<T>(aggregate.GetId());
            var changes = aggregate.Changes.ToArray();
            var events  = changes.Select(CreateEventData);

            var resultTask = aggregate.Version < 0
                ? _client.AppendToStreamAsync(stream, StreamState.NoStream, events)
                : _client.AppendToStreamAsync(stream, StreamRevision.FromInt64(aggregate.Version), events);
            var result     = await resultTask;

            aggregate.ClearChanges();

            static EventData CreateEventData(object e) {
                var meta = new EventMetadata {
                    ClrType = e.GetType().FullName
                };

                return new EventData(
                    Uuid.NewUuid(),
                    e.GetType().Name,
                    JsonSerializer.SerializeToUtf8Bytes(meta),
                    JsonSerializer.SerializeToUtf8Bytes(e)
                );
            }
        }

        public async Task<T> Load<T>(string id) where T : Aggregate {
            if (id == null) throw new ArgumentNullException(nameof(id));

            var stream = GetStreamName<T>(id);
            var aggregate = (T) Activator.CreateInstance(typeof(T), true);

            var read           = _client.ReadStreamAsync(Direction.Forwards, stream, StreamPosition.Start);
            var resolvedEvents = await read.ToArrayAsync();
            var events         = resolvedEvents.Select(x => x.Deserialize());

            aggregate!.Load(events);

            return aggregate;
        }

        static string GetStreamName<T>(string entityId) => $"{typeof(T).Name}-{entityId}";
    }
}
