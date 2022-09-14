namespace VacationRental.Domain.Calendar
{
    public class Calendar
    {
        private readonly List<CalendarDate> _dates;

        public int RentalId { get; }

        public IReadOnlyList<CalendarDate> Dates => _dates;

        public Calendar(int rentalId)
        {
            RentalId = rentalId;
            _dates = new List<CalendarDate>();
        }

        public void AddDate(CalendarDate calendarDate)
        {
            if (calendarDate == null)
                throw new ArgumentNullException(nameof(calendarDate));

            _dates.Add(calendarDate);
        }
    }
}