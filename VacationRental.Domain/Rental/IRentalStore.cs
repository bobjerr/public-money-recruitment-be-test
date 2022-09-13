namespace VacationRental.Domain.Rental;

public interface IRentalStore
{
    Rental Get(int id);

    int Create(int units);
}
