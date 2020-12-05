using System;

namespace DotNext.Domain {
    public static class BookingEvents {
        public static class V1 {
            public record RoomBooked {
                public string         BookingId { get; init; }
                public string         GuestId   { get; init; }
                public string         RoomId    { get; init; }
                public DateTimeOffset CheckIn   { get; init; }
                public DateTimeOffset CheckOut  { get; init; }
                public string         BookedBy  { get; init; }
                public DateTimeOffset BookedAt  { get; init; }
            }

            public record BookingExtended {
                public string         BookingId { get; init; }
                public DateTimeOffset CheckOut  { get; init; }
                public string         ExtendedBy { get; init; }
                public DateTimeOffset ExtendedAt { get; init; }
            }
        }
    }
}
