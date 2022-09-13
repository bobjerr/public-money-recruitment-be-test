using System.Collections.Concurrent;

namespace VacationRental.Domain.Booking.Create
{
    public class BookingLocker
    {
        private readonly ConcurrentDictionary<int, object> _lockers
            = new ConcurrentDictionary<int, object>();

        public object GetLocker(int rentalId)
            => _lockers.GetOrAdd(rentalId, new object());
    }
}
