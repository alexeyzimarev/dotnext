using DotNext.Domain;
using DotNext.Lib;

namespace DotNext.Application {
    public static class EventTypeMapper {
        public static void MapEventTypes() {
            TypeMap.AddType<BookingEvents.V1.RoomBooked>("RoomBooked");
            TypeMap.AddType<BookingEvents.V1.BookingExtended>("BookingExtended");
        }
    }
}
