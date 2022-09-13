namespace VacationRental.Domain.Booking.Get.Many;

public record Response(IEnumerable<Booking> Booking)
{
}
