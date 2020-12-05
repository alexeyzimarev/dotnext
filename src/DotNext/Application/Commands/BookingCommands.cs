using System;

namespace DotNext.Application.Commands {
    public static class BookingCommands {
        public static class V1 {
            public record BookRoom {
                public string         BookingId { get; set; }
                public string         GuestId   { get; init; }
                public string         RoomId    { get; init; }
                public DateTimeOffset CheckIn   { get; init; }
                public DateTimeOffset CheckOut  { get; init; }
            }

            public record ExtendBooking {
                public string         BookingId  { get; init; }
                public DateTimeOffset CheckOut   { get; init; }
            }
        }
    }
}
