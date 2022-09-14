using MediatR;

namespace VacationRental.Domain.Rental.Get;

public record Request(int Id) : IRequest<Response>
{
}
