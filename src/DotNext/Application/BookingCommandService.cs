using System.Threading.Tasks;
using DotNext.Domain;
using DotNext.Lib;
using static DotNext.Application.Commands.BookingCommands;

namespace DotNext.Application {
    public class BookingCommandService {
        readonly IAggregateStore    _store;
        readonly IAvailabilityCheck _availabilityCheck;

        public BookingCommandService(IAggregateStore store, IAvailabilityCheck availabilityCheck) {
            _store             = store;
            _availabilityCheck = availabilityCheck;
        }

        public async Task Handle(V1.BookRoom command) {
            var period = new StayPeriod(command.CheckIn, command.CheckOut);
            var roomId = new RoomId(command.RoomId);

            var exists = await _store.Exists<Booking>(command.BookingId);
            if (exists) throw new ApplicationException($"Booking with id {command.BookingId} already exists");

            var booking = new Booking();
            await booking.BookRoom(command.BookingId, command.GuestId, roomId, period, _availabilityCheck);
            await _store.Store(booking);
        }

        public async Task Handle(V1.ExtendBooking command) {
            var booking = await _store.Load<Booking>(command.BookingId);
            booking.ExtendStay(command.CheckOut);
            await _store.Store(booking);
        }
    }
}
