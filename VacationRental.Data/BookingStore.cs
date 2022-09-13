using VacationRental.Domain.Booking;

namespace VacationRental.Data
{
    public class BookingStore : IBookingStore
    {
        private readonly SequenceEmulator _sequenceEmulator;
        private readonly IDictionary<int, Booking> _bookings;

        public BookingStore()
        {
            _bookings = new Dictionary<int, Booking>();
            _sequenceEmulator = new SequenceEmulator();
        }

        public Booking Get(int id)
        {
            if (!_bookings.ContainsKey(id))
                throw new ApplicationException("Booking not found");

            return _bookings[id];
        }

        public IEnumerable<Booking> GetByRentalId(int rentalId)
        {
            return _bookings.Values.Where(b => b.RentalId == rentalId);
        }

        public int Save(Booking booking)
        {
            booking.Id = _sequenceEmulator.GetNextValue();
            _bookings[booking.Id] = booking;

            return booking.Id;
        }
    }
}
