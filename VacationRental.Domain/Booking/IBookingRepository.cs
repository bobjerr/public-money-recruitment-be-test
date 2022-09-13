using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationRental.Domain.Booking
{
    public interface IBookingRepository
    {
        Booking Get(int id);

        IEnumerable<Booking> GetByRentalId(int rentalId);

        int Save(Booking newBooking);
    }
}
