using MediatR;

namespace VacationRental.Domain.Rental.Update;

public class Command : IRequestHandler<Request, Unit>
{
    private readonly IMediator _mediator;
    private readonly IRentalRepository _rentalRepository;
    private readonly SemaphorService _semaphorService;

    public Command(IMediator mediator, IRentalRepository rentalRepository, SemaphorService semaphorService)
    {
        _mediator = mediator;
        _rentalRepository = rentalRepository;
        _semaphorService = semaphorService;
    }

    public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
    {
        if (request.PreparationTimeInDays < 0)
            throw new ArgumentOutOfRangeException("Preparation time must be positive");

        if (request.Units < 0)
            throw new ArgumentOutOfRangeException("Units must be greater than 0");

        var rental = await GetRental(request);

        await _semaphorService.GetSemaphor(request.RentalId).WaitAsync(cancellationToken);
        try
        {
            var bookings = await GetBookings(request);

            if (bookings.Any() && Overlaps(request, bookings))
            {
                throw new ApplicationException("Not Available.");
            }

            _rentalRepository.Update(new Rental(rental.Id, request.Units, request.PreparationTimeInDays));
        }
        finally
        {
            _semaphorService.GetSemaphor(request.RentalId).Release();
        }

        return Unit.Value;
    }

    private static bool Overlaps(Request request, IEnumerable<Booking.Booking> bookings)
        => bookings
            .SelectMany(b => Enumerable
                .Range(0, b.Nights + request.PreparationTimeInDays)
                .Select(x => b.Start.AddDays(x)))
            .GroupBy(x => x)
            .Any(x => x.Count() > request.Units);

    private async Task<IEnumerable<Booking.Booking>> GetBookings(Request request)
    {
        var response = await _mediator.Send(new Booking.Get.Many.Request(request.RentalId));
        return response.Bookings;
    }

    private async Task<Rental> GetRental(Request request)
    {
        var response = await _mediator.Send(new Get.Request(request.RentalId));
        return response.Rental;
    }
}
