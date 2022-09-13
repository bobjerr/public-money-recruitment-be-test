﻿using MediatR;

namespace VacationRental.Domain.Rental.Get;

public class Query : IRequestHandler<Request, Response>
{
    private readonly IRentalRepository _rentalStore;

    public Query(IRentalRepository rentalStore)
    {
        _rentalStore = rentalStore;
    }

    public Task<Response> Handle(Request request, CancellationToken cancellationToken)
        => Task.FromResult(new Response(_rentalStore.Get(request.Id)));
}
