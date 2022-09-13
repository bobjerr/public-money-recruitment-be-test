using MediatR;

namespace VacationRental.Domain.Booking.Get.Many;

public class Query : IRequestHandler<Request, Response>
{
    private readonly IBookingRepository _bookingStore;

    public Query(IBookingRepository bookingStore)
    {
        _bookingStore = bookingStore;
    }

    public Task<Response> Handle(Request request, CancellationToken cancellationToken) 
        => Task.FromResult(new Response(_bookingStore.GetByRentalId(request.RentalId)));
}
