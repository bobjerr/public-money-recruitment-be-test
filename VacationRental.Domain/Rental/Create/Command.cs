using MediatR;

namespace VacationRental.Domain.Rental.Create;

public class Command : IRequestHandler<Request, Response>
{
    private readonly IRentalRepository _rentalRepository;

    public Command(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        var id = _rentalRepository.Create(request.Units, request.PreparationTimeInDays);

        return Task.FromResult(new Response(id));
    }
}
