using System.Threading;
using System.Threading.Tasks;

namespace DotNext.Infrastructure.MongoDb
{
    public class MongoCheckpointStore : ICheckpointStore
    {
        static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        public MongoCheckpointStore(IMongoCollection<Checkpoint> database) => Checkpoints = database;

        public MongoCheckpointStore(IMongoDatabase database) : this(database.GetCollection<Checkpoint>("checkpoint")) { }

        IMongoCollection<Checkpoint> Checkpoints { get; }

        public async ValueTask<Checkpoint> GetLastCheckpoint(string checkpointId, CancellationToken cancellationToken = default) {
            Log.Debug("[{CheckpointId}] Finding checkpoint ...", checkpointId);

            var checkpoint = await Checkpoints.AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == checkpointId, cancellationToken);

            if (checkpoint is null) {
                checkpoint = Checkpoint.Start(checkpointId);
                Log.Info("[{CheckpointId}] Checkpoint not found. Defaulting to earliest position.", checkpointId);
            }
            else {
                Log.Info("[{CheckpointId}] Checkpoint found at position {Checkpoint}", checkpointId, checkpoint.Position);
            }

            return checkpoint;
        }

        public async ValueTask<Checkpoint> StoreCheckpoint(Checkpoint checkpoint, CancellationToken cancellationToken = default) {
            await Checkpoints.ReplaceOneAsync(
                x => x.Id == checkpoint.Id,
                checkpoint,
                MongoDefaults.DefaultReplaceOptions,
                cancellationToken
            );

            Log.Debug("[{CheckpointId}] Checkpoint position set to {Checkpoint}", checkpoint.Id, checkpoint.Position);

            return checkpoint;
        }
    }
}
