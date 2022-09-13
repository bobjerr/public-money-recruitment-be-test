namespace VacationRental.Domain.Rental;

public class Rental
{
    public int Id { get; }

    public int Units { get; }

    public Rental(int id, int units)
    {
        if (units < 0)
            throw new ArgumentOutOfRangeException(nameof(units), "Should be positive");

        Id = id;
        Units = units;
    }
}
