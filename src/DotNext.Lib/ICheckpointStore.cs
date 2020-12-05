using System.Threading;
using System.Threading.Tasks;

namespace DotNext.Lib {
    public interface ICheckpointStore {
        ValueTask<Checkpoint> GetLastCheckpoint(string checkpointId, CancellationToken cancellationToken = default);

        ValueTask<Checkpoint> StoreCheckpoint(Checkpoint checkpoint, CancellationToken cancellationToken = default);
    }
}
