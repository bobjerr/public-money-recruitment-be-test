using VacationRental.Domain.Rental;

namespace VacationRental.Data;

public class RentalStore : IRentalStore
{
    private readonly IDictionary<int, Rental> _rentals;

    public RentalStore()
    {
        _rentals = new Dictionary<int, Rental>();
    }

    public Rental Get(int id)
    {
        if (!_rentals.ContainsKey(id))
            throw new ApplicationException("Rental not found");

        return _rentals[id];
    }

    public int Create(int units)
    {
        var id = _rentals.Keys.Count + 1;

        _rentals.Add(id, new Rental(id, units));

        return id;
    }
}
