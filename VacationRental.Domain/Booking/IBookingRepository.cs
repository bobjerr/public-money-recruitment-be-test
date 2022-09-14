namespace VacationRental.Domain.Booking
{
    public interface IBookingRepository
    {
        Booking Get(int id);

        IEnumerable<Booking> GetByRentalId(int rentalId);

        int Save(Booking newBooking);
    }
}
