using System.Threading.Tasks;

namespace DotNext.Lib {
    public interface IEventHandler {
        Task HandleEvent(object evt);
    }
}
