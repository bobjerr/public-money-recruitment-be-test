namespace VacationRental.Domain.Rental;

public class Rental
{
    public int Id { get; }

    public int Units { get; }

    public int PreparationTimeInDays { get; }

    public Rental(int id, int units, int preparationTimeInDays)
    {
        if (units < 0)
            throw new ArgumentOutOfRangeException(nameof(units), "Must be positive");

        if (preparationTimeInDays < 0)
            throw new ArgumentOutOfRangeException(nameof(preparationTimeInDays), "Must be positive");

        Id = id;
        Units = units;
        PreparationTimeInDays = preparationTimeInDays;
    }
}
