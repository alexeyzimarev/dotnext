using System.Threading.Tasks;

namespace DotNext.Lib {
    public interface IAggregateStore {
        Task Store<T>(T aggregate) where T : Aggregate;

        Task<T> Load<T>(string id) where T : Aggregate;

        Task<bool> Exists<T>(string id);
    }
}
