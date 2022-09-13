using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<BookingViewModel> Get(int bookingId)
        {
            var result = await _mediator.Send(new Domain.Booking.Get.Request(bookingId));

            return new BookingViewModel
            {
                Id = result.Booking.Id,
                RentalId = result.Booking.RentalId,
                Nights = result.Booking.Nights,
                Start = new DateTime(result.Booking.Start.Year, result.Booking.Start.Month, result.Booking.Start.Day)
            };
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post(BookingBindingModel model)
        {
            var response = await _mediator.Send(new Domain.Booking.Create.Request(model.RentalId, DateOnly.FromDateTime(model.Start), model.Nights));

            return new ResourceIdViewModel
            {
                Id = response.Id
            };
        }
    }
}
