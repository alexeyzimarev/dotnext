using System.Threading.Tasks;
using DotNext.Domain;
using DotNext.Lib;
using static DotNext.Application.Commands.BookingCommands;

namespace DotNext.Application {
    public class BookingCommandService {
        readonly IAggregateStore _store;
        
        public BookingCommandService(IAggregateStore store) => _store = store;

        public async Task Handle(V1.BookRoom command) {
            var period = new StayPeriod(command.CheckIn, command.CheckOut);
            var roomId = new RoomId(command.RoomId);
            
            var exists = await _store.Exists<Booking>(command.BookingId);
            if (exists) throw new ApplicationException($"Booking with id {command.BookingId} already exists");

            var booking = new Booking();
            booking.BookRoom(command.BookingId, command.GuestId, roomId, period);
            await _store.Store(booking);
        }
    }
}
