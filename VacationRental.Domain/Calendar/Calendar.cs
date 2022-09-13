namespace VacationRental.Domain.Calendar
{
    public class Calendar
    {
        private List<CalendarDate> _dates;

        public int RentalId { get; }

        public IReadOnlyCollection<CalendarDate> Dates => _dates;

        public Calendar(int rentalId)
        {
            RentalId = rentalId;
            _dates = new List<CalendarDate>();
        }

        public void AddDate(DateOnly date, IEnumerable<Booking.Booking> bookings)
        {
            var calendarDate = new CalendarDate(date, bookings);
            _dates.Add(calendarDate);
        }
    }
}