using MediatR;

namespace VacationRental.Domain.Booking.Create;

public record Request(int RentalId, DateOnly Start, int Nights) : IRequest<Response>
{
}
