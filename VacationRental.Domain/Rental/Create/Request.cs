using MediatR;

namespace VacationRental.Domain.Rental.Create;

public record Request(int Units) : IRequest<Response>
{
}
