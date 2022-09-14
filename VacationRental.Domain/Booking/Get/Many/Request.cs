using MediatR;

namespace VacationRental.Domain.Booking.Get.Many;

public record Request(int RentalId) : IRequest<Response>
{
}
