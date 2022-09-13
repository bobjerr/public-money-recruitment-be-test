using MediatR;

namespace VacationRental.Domain.Rental.Create;

public class Command : IRequestHandler<Request, Response>
{
    private readonly IRentalRepository _rentalStore;

    public Command(IRentalRepository rentalStore)
    {
        _rentalStore = rentalStore;
    }

    public Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        var id = _rentalStore.Create(request.Units, request.PreparationTimeInDays);

        return Task.FromResult(new Response(id));
    }
}
