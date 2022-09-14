using MediatR;

namespace VacationRental.Domain.Calendar.Get;

public class Query : IRequestHandler<Request, Response>
{
    private readonly IMediator _mediator;

    public Query(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        if (request.Nights < 0)
            throw new ApplicationException("Nights must be positive");

        var rental = await GetRental(request.RentalId);

        var calendar = new Calendar(request.RentalId);

        for (var i = 0; i < request.Nights; i++)
        {
            var date = request.Start.AddDays(i);

            var bookings = await GetBookings(request.RentalId);

            var calendarDate = new CalendarDate(date);
            foreach (var booking in bookings)
            {
                if (booking.Start <= date && booking.Start.AddDays(booking.Nights) > date)
                {
                    calendarDate.AddBooking(booking);
                }
                else if (booking.End <= date && booking.End.AddDays(rental.PreparationTimeInDays) > date)
                {
                    calendarDate.AddPreparation(booking.Unit);
                }
            }

            calendar.AddDate(calendarDate);
        }

        return new Response(calendar);
    }

    private async Task<IEnumerable<Booking.Booking>> GetBookings(int rentalId)
    {
        var response = await _mediator.Send(new Booking.Get.Many.Request(rentalId));
        return response.Bookings;
    }

    private async Task<Rental.Rental> GetRental(int rentalId)
    {
        var response = await _mediator.Send(new Rental.Get.Request(rentalId));
        return response.Rental;
    }
}
