using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNext.Lib;
using EventStore.Client;
using Microsoft.Extensions.Hosting;

namespace DotNext.Infrastructure.MongoDb {
    public class MongoProjectionService : IHostedService {
        readonly EventStoreClient _eventStoreClient;
        readonly ICheckpointStore _checkpointStore;
        readonly IEventHandler[]  _projections;
        readonly string           _checkpointId;

        Checkpoint         _checkpoint;
        StreamSubscription _subscription;

        public MongoProjectionService(
            EventStoreClient eventStoreClient,
            ICheckpointStore checkpointStore,
            string checkpointId,
            params IEventHandler[] projections
        ) {
            _eventStoreClient = eventStoreClient;
            _checkpointStore  = checkpointStore;
            _checkpointId     = checkpointId;
            _projections       = projections;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            _checkpoint = await _checkpointStore.GetLastCheckpoint(_checkpointId, cancellationToken);

            var position = _checkpoint.Position != null
                ? new Position((ulong) _checkpoint.Position.Value, (ulong) _checkpoint.Position.Value)
                : Position.Start;

            _subscription = await _eventStoreClient.SubscribeToAllAsync(
                position,
                Handler,
                cancellationToken: cancellationToken
            );
        }

        async Task Handler(StreamSubscription sub, ResolvedEvent re, CancellationToken cancellationToken) {
            if (re.Event.EventType.StartsWith("$")) return;

            var evt = re.Deserialize();
            await Task.WhenAll(_projections.Select(x => x.HandleEvent(evt)));
            _checkpoint.Position = (long?) re.Event.Position.CommitPosition;
            await _checkpointStore.StoreCheckpoint(_checkpoint, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            _subscription.Dispose();
            return Task.CompletedTask;
        }
    }
}
