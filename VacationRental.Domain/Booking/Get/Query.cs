using MediatR;

namespace VacationRental.Domain.Booking.Get;

public class Query : IRequestHandler<Request, Response>
{
    private readonly IBookingStore _bookingStore;

    public Query(IBookingStore bookingStore)
    {
        _bookingStore = bookingStore;
    }

    public Task<Response> Handle(Request request, CancellationToken cancellationToken) 
        => Task.FromResult(new Response(_bookingStore.Get(request.Id)));
}
