using MediatR;

namespace VacationRental.Domain.Booking.Get;

public class Query : IRequestHandler<Request, Response>
{
    private readonly IBookingRepository _bookingRepository;

    public Query(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public Task<Response> Handle(Request request, CancellationToken cancellationToken)
        => Task.FromResult(new Response(_bookingRepository.Get(request.Id)));
}
