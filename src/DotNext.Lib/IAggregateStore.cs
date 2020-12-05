using System.Threading.Tasks;
using EventSourcing.Infrastructure;

namespace DotNext.Lib
{
    public interface IAggregateStore
    {
        Task Store<T>(T aggregate) where T : Aggregate;

        Task<T> Load<T>(string id) where T : Aggregate;

        // Task<bool> Exists(string id);
    }
}
