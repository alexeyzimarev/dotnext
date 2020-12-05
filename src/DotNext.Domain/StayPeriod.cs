using System;

namespace DotNext.Domain {
    public record StayPeriod {
        public DateTimeOffset CheckIn  { get; }
        public DateTimeOffset CheckOut { get; }

        public StayPeriod(DateTimeOffset checkIn, DateTimeOffset checkOut) {
            if (checkIn > checkOut) throw new DomainException("Check in date must be before check out date");
            CheckIn  = checkIn;
            CheckOut = checkOut;
        }
    }
}
