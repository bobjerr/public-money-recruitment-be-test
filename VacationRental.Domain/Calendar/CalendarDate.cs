namespace VacationRental.Domain.Calendar
{
    public class CalendarDate
    {
        private readonly List<CalendarBooking> _bookings;
        private readonly List<Preparation> _preparations;

        public DateOnly Date { get; }

        public IReadOnlyCollection<CalendarBooking> Bookings => _bookings;

        public IReadOnlyCollection<Preparation> Preparations => _preparations;

        public CalendarDate(DateOnly date)
        {
            Date = date;
            _bookings = new List<CalendarBooking>();
            _preparations = new List<Preparation>();
        }

        public void AddBooking(Booking.Booking booking)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(booking));

            _bookings.Add(new CalendarBooking(booking.Id, booking.Unit));
        }

        public void AddPreparation(int unit)
        {
            _preparations.Add(new Preparation(unit));
        }
    }
}
