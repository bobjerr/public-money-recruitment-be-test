namespace VacationRental.Domain.Calendar
{
    public class CalendarDate
    {
        public DateOnly Date { get; }
        public IReadOnlyCollection<CalendarBooking> Bookings { get; }

        public CalendarDate(DateOnly date, IEnumerable<Booking.Booking> bookings)
        {
            Date = date;
            Bookings = bookings
                .Select(b => new CalendarBooking(b.Id))
                .ToList();
        }
    }
}
