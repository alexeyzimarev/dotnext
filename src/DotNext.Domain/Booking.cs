using System;
using System.Threading.Tasks;
using DotNext.Lib;
using static DotNext.Domain.BookingEvents;

namespace DotNext.Domain {
    public class Booking : Aggregate {
        string         _id;
        DateTimeOffset _checkIn;
        DateTimeOffset _checkOut;

        public override string GetId() => _id;

        public async Task BookRoom(string bookingId, string guestId, RoomId roomId, StayPeriod period, IAvailabilityCheck availabilityCheck) {
            EnsureDoesntExist();
            await EnsureRoomAvailable(roomId, period, availabilityCheck);

            Apply(
                new V1.RoomBooked {
                    BookingId = bookingId,
                    GuestId   = guestId,
                    RoomId    = roomId.Value,
                    CheckIn   = period.CheckIn,
                    CheckOut  = period.CheckOut
                }
            );
        }
        
        public void ExtendStay(in DateTimeOffset checkOut) {
            // Ensure the room is available for the extended period
            
            Apply(new V1.BookingExtended {
                BookingId = _id,
                CheckOut = checkOut
            });
        }

        void EnsureDoesntExist() {
            if (Version > -1) throw new DomainException("Booking already exists");
        }

        static async Task EnsureRoomAvailable(RoomId roomId, StayPeriod period, IAvailabilityCheck availabilityCheck) {
            var isRoomAvailable = await availabilityCheck.IsRoomAvailable(roomId, period);
            if (!isRoomAvailable) throw new DomainException("Room not available");
        }

        protected override void When(object evt) {
            switch (evt) {
                case V1.RoomBooked e: {
                    _id       = e.BookingId;
                    _checkIn  = e.CheckIn;
                    _checkOut = e.CheckOut;
                    break;
                }
                case V1.BookingExtended e: {
                    _checkOut = e.CheckOut;
                    break;
                }
            }
        }

    }
}
