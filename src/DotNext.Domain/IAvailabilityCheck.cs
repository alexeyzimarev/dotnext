using System.Threading.Tasks;

namespace DotNext.Domain {
    public interface IAvailabilityCheck {
        Task<bool> IsRoomAvailable(RoomId roomId, StayPeriod period);
    }
}
