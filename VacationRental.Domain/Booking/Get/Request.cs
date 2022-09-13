using MediatR;

namespace VacationRental.Domain.Booking.Get;

public record Request(int Id) : IRequest<Response>
{
}
