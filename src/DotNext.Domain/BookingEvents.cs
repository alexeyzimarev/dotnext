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
                
                public override string ToString() => $"Room {RoomId} booked for {GuestId} from {CheckIn} to {CheckOut}";
            }

            public record BookingExtended {
                public string         BookingId { get; init; }
                public DateTimeOffset CheckOut  { get; init; }
                public string         ExtendedBy { get; init; }
                public DateTimeOffset ExtendedAt { get; init; }

                public override string ToString() => $"Booking {BookingId} extended to {CheckOut}";
            }
        }
        
        public static class V2 {
            public record BookingExtended {
                public string         BookingId  { get; init; }
                public string         GuestId    { get; init; }
                public DateTimeOffset CheckOut   { get; init; }
                public string         ExtendedBy { get; init; }
                public DateTimeOffset ExtendedAt { get; init; }
                
                public override string ToString() => $"Booking {BookingId} for {GuestId} extended to {CheckOut}";
            }
            
            public record BookingShrink {
                public string         BookingId  { get; init; }
                public string         GuestId    { get; init; }
                public DateTimeOffset CheckOut   { get; init; }
                public string         ExtendedBy { get; init; }
                public DateTimeOffset ExtendedAt { get; init; }
            }
        }
    }
}
