using System.Threading.Tasks;
using DotNext.Domain;
using Microsoft.Extensions.Logging;

namespace DotNext.Application {
    public class FakeAvailabilityCheck : IAvailabilityCheck {
        readonly ILogger<FakeAvailabilityCheck> _logger;
        
        public FakeAvailabilityCheck(ILogger<FakeAvailabilityCheck> logger) => _logger = logger;
        
        public Task<bool> IsRoomAvailable(RoomId roomId, StayPeriod period) {
            _logger.LogInformation($"Checking availability for room {roomId}");
            return Task.FromResult(true);
        }
    }
}
