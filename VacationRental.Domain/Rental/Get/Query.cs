using MediatR;

namespace VacationRental.Domain.Rental.Get;

public class Query : IRequestHandler<Request, Response>
{
    private readonly IRentalRepository _rentalRepository;

    public Query(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public Task<Response> Handle(Request request, CancellationToken cancellationToken)
        => Task.FromResult(new Response(_rentalRepository.Get(request.Id)));
}
