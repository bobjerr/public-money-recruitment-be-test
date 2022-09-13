using MediatR;

namespace VacationRental.Domain.Booking.Create;

public class Command : IRequestHandler<Request, Response>
{
    private readonly IMediator _mediator;
    private readonly IBookingStore _bookingStore;

    public Command(IMediator mediator, IBookingStore bookingStore)
    {
        _mediator = mediator;
        _bookingStore = bookingStore;
    }
    
    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        if (request.Nights < 0)
            throw new ApplicationException("Nights must be positive");

        var newBooking = new Booking
        {
            Start = request.Start,
            Nights = request.Nights,
            RentalId = request.RentalId
        };

        var rental = await GetRental(request.RentalId);

        var count = 0;

        //todo: fix situation when several users try to book one rentalId at the same time
        var bookings = await GetBookings(request.RentalId);
        foreach (var booking in bookings)
        {
            if (newBooking.Overlap(booking))
            {
                count++;

                if (count >= rental.Units)
                    throw new ApplicationException("Not available");
            }
        }

        var id = SaveBooking(newBooking);

        return new Response(id);
    }

    private async Task<IEnumerable<Booking>> GetBookings(int rentalId)
    {
        var response = await _mediator.Send(new Get.Many.Request(rentalId));
        return response.Booking;
    }

    private async Task<Rental.Rental> GetRental(int rentalId)
    {
        var response = await _mediator.Send(new Rental.Get.Request(rentalId));
        return response.Rental;
    }

    private int SaveBooking(Booking newBooking)
    {
        return _bookingStore.Save(newBooking);
    }
}
