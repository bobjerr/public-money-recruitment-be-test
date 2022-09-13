using MediatR;

namespace VacationRental.Domain.Rental.Create;

public class Command : IRequestHandler<Request, Response>
{
    private readonly IRentalStore _rentalStore;

    public Command(IRentalStore rentalStore)
    {
        _rentalStore = rentalStore;
    }

    public Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        var id = _rentalStore.Create(request.Units);

        return Task.FromResult(new Response(id));
    }
}
