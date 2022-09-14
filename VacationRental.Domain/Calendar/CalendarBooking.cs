namespace VacationRental.Domain.Calendar
{
    public class CalendarBooking
    {
        public int Id { get; }
        public int Unit { get; }

        public CalendarBooking(int id, int unit)
        {
            Id = id;
            Unit = unit;
        }
    }
}
