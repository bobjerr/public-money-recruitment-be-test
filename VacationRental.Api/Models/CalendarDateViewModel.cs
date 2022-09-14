namespace VacationRental.Api.Models;

public class CalendarDateViewModel
{
    public DateTime Date { get; set; }
    public List<CalendarBookingViewModel> Bookings { get; set; } = new List<CalendarBookingViewModel>();
    public List<PreparationViewModel> PreparationTimes { get; set; } = new List<PreparationViewModel>();
}
