using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNext.Infrastructure.MongoDb;
using DotNext.Projections;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DotNext.Api {
    [Route("/")]
    public class GuestBookingsQueries : ControllerBase {
        readonly IMongoCollection<GuestBookings> _collection;

        public GuestBookingsQueries(IMongoDatabase database) 
            => _collection = database.GetDocumentCollection<GuestBookings>();

        [HttpGet]
        [Route("/guest/{GuestId}/bookings")]
        public async Task<GuestBookingsQuery.Result> GuestBookings(GuestBookingsQuery query) {
            var doc = await _collection.LoadDocument(query.GuestId);

            return new GuestBookingsQuery.Result {
                GuestId = doc.Id,
                GuestBookings = doc.GuestBooking.Select(
                        x => new GuestBookingsQuery.Result.GuestBooking {
                            BookingId    = x.BookingId,
                            CheckInDate  = x.CheckInDate,
                            CheckOutDate = x.CheckOutDate
                        }
                    )
                    .ToList()
            };
        }
    }

    public class GuestBookingsQuery {
        [FromRoute] 
        public string GuestId { get; set; }

        public class Result {
            public string             GuestId       { get; set; }
            public List<GuestBooking> GuestBookings { get; set; } = new();

            public record GuestBooking {
                public string BookingId    { get; init; }
                public string CheckInDate  { get; init; }
                public string CheckOutDate { get; init; }
            }
        }
    }
}
