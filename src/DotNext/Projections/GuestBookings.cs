using System.Collections.Generic;
using DotNext.Lib;

namespace DotNext.Projections {
    public class GuestBookings : Document {
        public List<GuestBooking> GuestBooking { get; set; } = new();
    }

    public record GuestBooking {
        public string BookingId    { get; init; }
        public string CheckInDate  { get; init; }
        public string CheckOutDate { get; init; }
    }
}
