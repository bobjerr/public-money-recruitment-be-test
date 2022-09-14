using MediatR;

namespace VacationRental.Domain.Rental.Update;

public record Request(int RentalId, int Units, int PreparationTimeInDays) : IRequest<Unit>
{
}
