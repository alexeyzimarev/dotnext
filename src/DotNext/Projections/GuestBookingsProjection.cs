using System.Threading.Tasks;
using DotNext.Infrastructure.MongoDb;
using DotNext.Lib;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using static DotNext.Domain.BookingEvents;
using static MongoDB.Driver.Builders<DotNext.Projections.GuestBookings>;

namespace DotNext.Projections {
    public class GuestBookingsProjection : IEventHandler {
        readonly ILogger<GuestBookingsProjection> _logger;
        readonly IMongoCollection<GuestBookings>  _collection;

        public GuestBookingsProjection(IMongoDatabase database, ILogger<GuestBookingsProjection> logger) {
            _logger     = logger;
            _collection = database.GetDocumentCollection<GuestBookings>();
        }

        public async Task HandleEvent(object evt) {
            _logger.LogInformation("Projecting {Event}", evt);

            var update = When(evt);
            if (update == null) return;

            await _collection.UpdateDocument(update.Filter, update.Update);
        }

        static MongoUpdate<GuestBookings> When(object evt)
            => evt switch {
                V1.RoomBooked e => new MongoUpdate<GuestBookings> {
                    Update = Update
                        .SetOnInsert(x => x.Id, e.GuestId)
                        .AddToSet(
                            x => x.GuestBooking,
                            new GuestBooking {BookingId = e.BookingId, CheckInDate = e.CheckIn.ToString("d"), CheckOutDate = e.CheckOut.ToString("d")}
                        ),
                    Filter = Filter.Eq(x => x.Id, e.GuestId)
                },
                _ => null
            };
    }

    public record MongoUpdate<T> where T : Document {
        public FilterDefinition<T> Filter { get; init; }
        public UpdateDefinition<T> Update { get; init; }
    }
}
