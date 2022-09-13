using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers;

[Route("api/v1/calendar")]
[ApiController]
public class CalendarController : ControllerBase
{
    private readonly IMediator _mediator;

    public CalendarController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
    {
        var calendarResponse = await _mediator.Send(new Domain.Calendar.Get.Request(nights, rentalId, DateOnly.FromDateTime(start)));

        return new CalendarViewModel
        {
            RentalId = calendarResponse.Calendar.RentalId,
            Dates = calendarResponse.Calendar.Dates
                .Select(d => new CalendarDateViewModel
                {
                    Date = d.Date.ToDateTime(TimeOnly.MinValue),
                    Bookings = d.Bookings.Select(b => new CalendarBookingViewModel
                    {
                        Id = b.Id
                    }).ToList()
                }).ToList()
        };
    }
}
