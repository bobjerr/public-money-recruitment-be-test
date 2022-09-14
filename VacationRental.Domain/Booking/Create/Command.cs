using MediatR;

namespace VacationRental.Domain.Booking.Create;

public class Command : IRequestHandler<Request, Response>
{
    private readonly IMediator _mediator;
    private readonly IBookingRepository _bookingRepository;
    private readonly SemaphorService _semaphorService;

    public Command(IMediator mediator, IBookingRepository bookingRepository, SemaphorService semaphorService)
    {
        _mediator = mediator;
        _bookingRepository = bookingRepository;
        _semaphorService = semaphorService;
    }

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        if (request.Nights < 0)
            throw new ApplicationException("Nights must be positive");

        var newBooking = new Booking(request.RentalId, request.Start, request.Nights);

        var rental = await GetRental(request.RentalId);

        var count = 0;

        var bookings = await GetBookings(request.RentalId);

        await _semaphorService.GetSemaphor(request.RentalId).WaitAsync(cancellationToken);
        try
        {
            var units = Enumerable.Range(1, rental.Units).ToHashSet();
            foreach (var booking in bookings)
            {
                if (newBooking.Overlap(booking, rental.PreparationTimeInDays))
                {
                    count++;
                    units.Remove(booking.Unit);

                    if (count >= rental.Units)
                        throw new ApplicationException("Not available");
                }
            }

            newBooking.Unit = units.Min();
            var id = SaveBooking(newBooking);

            return new Response(id);
        }
        finally
        {
            _semaphorService.GetSemaphor(request.RentalId).Release();
        }
    }

    private async Task<IEnumerable<Booking>> GetBookings(int rentalId)
    {
        var response = await _mediator.Send(new Get.Many.Request(rentalId));
        return response.Bookings;
    }

    private async Task<Rental.Rental> GetRental(int rentalId)
    {
        var response = await _mediator.Send(new Rental.Get.Request(rentalId));
        return response.Rental;
    }

    private int SaveBooking(Booking newBooking)
    {
        return _bookingRepository.Save(newBooking);
    }
}
