using MediatR;

namespace VacationRental.Domain.Calendar.Get;

public record Request(int Nights, int RentalId, DateOnly Start) : IRequest<Response>
{
}
