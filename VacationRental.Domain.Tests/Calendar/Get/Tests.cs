using FluentAssertions;
using MediatR;
using Moq;
using VacationRental.Domain.Booking;
using VacationRental.Domain.Calendar;
using VacationRental.Domain.Calendar.Get;

namespace VacationRental.Domain.Tests.Calendar.Get;

public class Tests
{

    [Fact]
    public async Task Invalid_number_of_nights()
    {
        var handler = new Query(Mock.Of<IMediator>());

        var exception = await Assert.ThrowsAsync<ApplicationException>(async ()
            => await handler.Handle(new Request(-5, 1, new DateOnly(2022, 2, 20)), CancellationToken.None));

        Assert.Equal("Nights must be positive", exception.Message);
    }

    [Theory]
    [MemberData(nameof(SuccessData))]
    public async Task Success(int rentalId, List<Domain.Booking.Booking> bookings, Domain.Calendar.Calendar expected)
    {
        var bookingRepositoryMock = GetBookingRepositoryMock(rentalId, bookings);

        var mediatorMock = GetMediatorMock(rentalId, bookingRepositoryMock.Object);

        var handler = new Query(mediatorMock.Object);

        var response = await handler.Handle(new Request(2, 1, new DateOnly(2022, 2, 27)), CancellationToken.None);

        Assert.Equal(rentalId, response.Calendar.RentalId);
        Assert.Equal(expected.Dates.Count, response.Calendar.Dates.Count);

        for (var i = 0; i < expected.Dates.Count; i++)
        {
            Assert.Equal(expected.Dates[i].Date, response.Calendar.Dates[i].Date);

            Assert.Equal(expected.Dates[i].Preparations.Count, response.Calendar.Dates[i].Preparations.Count);

            response.Calendar.Dates[i].Preparations.Should().BeEquivalentTo(expected.Dates[i].Preparations);

            Assert.Equal(expected.Dates[i].Bookings.Count, response.Calendar.Dates[i].Bookings.Count);

            response.Calendar.Dates[i].Bookings.Should().BeEquivalentTo(expected.Dates[i].Bookings);
        }
    }

    private static Mock<IBookingRepository> GetBookingRepositoryMock(int rentalId, List<Domain.Booking.Booking> bookings)
    {
        var bookingRepository = new Mock<IBookingRepository>();

        bookingRepository
            .Setup(x => x.GetByRentalId(It.Is<int>(x => x == rentalId)))
            .Returns(bookings);

        return bookingRepository;
    }

    private static Mock<IMediator> GetMediatorMock(int rentalId, IBookingRepository bookingRepository)
    {
        var mediator = new Mock<IMediator>();

        var query = new Domain.Booking.Get.Many.Query(bookingRepository);

        mediator
            .Setup(x => x.Send(It.IsAny<Domain.Booking.Get.Many.Request>(), It.IsAny<CancellationToken>()))
            .Returns(async () => await query.Handle(new Domain.Booking.Get.Many.Request(rentalId), CancellationToken.None));

        mediator
            .Setup(x => x.Send(It.IsAny<Domain.Rental.Get.Request>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new Domain.Rental.Get.Response(new Domain.Rental.Rental(rentalId, 5, 1))));

        return mediator;
    }

    public static IEnumerable<object[]> SuccessData()
    {
        var rentalId = 1;

        var expected = new Domain.Calendar.Calendar(rentalId);
        expected.AddDate(new CalendarDate(new DateOnly(2022, 2, 27)));
        expected.AddDate(new CalendarDate(new DateOnly(2022, 2, 28)));

        yield return new object[] { rentalId, new List<Domain.Booking.Booking>(), expected};

        var bookings = new List<Domain.Booking.Booking>();
        bookings.Add(new Domain.Booking.Booking(2, rentalId, new DateOnly(2022, 2, 20), 4) { Unit = 1 });
        bookings.Add(new Domain.Booking.Booking(3, rentalId, new DateOnly(2022, 2, 27), 1) { Unit = 1 });

        expected = new Domain.Calendar.Calendar(rentalId);
        var calendarDate = new CalendarDate(new DateOnly(2022, 2, 27));
        calendarDate.AddBooking(new Domain.Booking.Booking(3, rentalId, new DateOnly(2022, 2, 27), 1) {  Unit = 1 });
        expected.AddDate(calendarDate);

        calendarDate = new CalendarDate(new DateOnly(2022, 2, 28));
        calendarDate.AddPreparation(1);
        expected.AddDate(calendarDate);

        yield return new object[] { rentalId, bookings, expected };
    }
}