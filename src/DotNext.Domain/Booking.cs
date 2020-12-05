using System;
using DotNext.Lib;
using static DotNext.Domain.BookingEvents;

namespace DotNext.Domain {
    public class Booking : Aggregate {
        string _id;

        public override string GetId() => _id;

        public void BookRoom(string bookingId, string guestId, RoomId roomId, StayPeriod period) {
            EnsureDoesntExist();
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

        void EnsureDoesntExist() {
            if (Version > -1) throw new DomainException("Booking already exists");
        }

        protected override void When(object evt) {
            switch (evt) {
                case V1.RoomBooked e: {
                    _id = e.BookingId;
                    break;
                }
            }
        }
    }
}
