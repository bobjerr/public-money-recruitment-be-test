using VacationRental.Domain.Rental;

namespace VacationRental.Data;

public class RentalRepository : IRentalRepository
{
    private readonly IDictionary<int, Rental> _rentals;

    public RentalRepository()
    {
        _rentals = new Dictionary<int, Rental>();
    }

    public Rental Get(int id)
    {
        if (!_rentals.ContainsKey(id))
            throw new ApplicationException("Rental not found");

        return _rentals[id];
    }

    public int Create(int units, int preparationTimeInDays)
    {
        var id = _rentals.Keys.Count + 1;

        _rentals.Add(id, new Rental(id, units, preparationTimeInDays));

        return id;
    }

    public void Update(Rental rental)
    {
        if (rental == null)
            throw new ArgumentNullException(nameof(rental));

        _rentals[rental.Id] = rental;
    }
}
