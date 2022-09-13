namespace VacationRental.Domain.Rental;

public interface IRentalRepository
{
    Rental Get(int id);

    int Create(int units, int preparationTimeInDays);
}
