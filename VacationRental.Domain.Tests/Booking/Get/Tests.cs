using Moq;
using VacationRental.Domain.Booking;

namespace VacationRental.Domain.Tests.Booking.Get;

public class Tests
{
    [Theory]
    [InlineData(5)]
    public async Task SuccessfullyGetBooking(int id)
    {
        var booking = new Domain.Booking.Booking(id, 1, new DateOnly(2003, 2, 2), 5);

        var bookingRepository = new Mock<IBookingRepository>();

        bookingRepository
            .Setup(x => x.Get(It.Is<int>(x => x == id)))
            .Returns(booking);

        var handler = new Domain.Booking.Get.Query(bookingRepository.Object);

        var response = await handler.Handle(new Domain.Booking.Get.Request(id), CancellationToken.None);

        Assert.Equal(booking.Id, response.Booking.Id);
        Assert.Equal(booking.RentalId, response.Booking.RentalId);
        Assert.Equal(booking.Start, response.Booking.Start);
        Assert.Equal(booking.Nights, response.Booking.Nights);
    }
}
