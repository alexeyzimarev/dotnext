using System.Threading.Tasks;
using DotNext.Application;
using Microsoft.AspNetCore.Mvc;
using static DotNext.Application.Commands.BookingCommands;

namespace DotNext.Api {
    [Route("/booking")]
    public class BookingCommandApi : ControllerBase {
        readonly BookingCommandService _service;
        
        public BookingCommandApi(BookingCommandService service) => _service = service;

        [HttpPost]
        public Task BookRoom([FromBody] V1.BookRoom command) => _service.Handle(command);
        
        [HttpPost]
        [Route("extend")]
        public Task ExtendStay([FromBody] V1.ExtendBooking command) => _service.Handle(command);
    }
}
