using MediatR;
using Moq;
using VacationRental.Domain.Booking;

namespace VacationRental.Domain.Tests.Booking.Create;

public class FailedCreation
{
    [Fact]
    public async Task InvalidNumberOfNights()
    {
        var handler = new Domain.Booking.Create.Command(Mock.Of<IMediator>(), Mock.Of<IBookingStore>());

        var exception = await Assert.ThrowsAsync<ApplicationException>(async ()
            => await handler.Handle(new Domain.Booking.Create.Request(5, new DateOnly(2022, 2, 20), -5), CancellationToken.None));

        Assert.Equal("Nights must be positive", exception.Message);
    }


    [Theory]
    [MemberData(nameof(SuccessCreationData))]
    public async Task FailedBookingCreation(int id, List<Domain.Booking.Booking> bookings)
    {
        int rentalId = 1;

        var bookingStoreMock = GetBookingStoreMock(id, rentalId, bookings);

        var mediatorMock = GetMediatorMock(rentalId, bookingStoreMock);

        var handler = new Domain.Booking.Create.Command(mediatorMock.Object, bookingStoreMock.Object);

        var exception = await Assert.ThrowsAsync<ApplicationException>(async () 
            => await handler.Handle(new Domain.Booking.Create.Request(5, new DateOnly(2022, 2, 20), 5), CancellationToken.None));

        Assert.Equal("Not available", exception.Message);
    }

    private static Mock<IBookingStore> GetBookingStoreMock(int id, int rentalId, List<Domain.Booking.Booking> bookings)
    {
        var bookingStore = new Mock<IBookingStore>();

        bookingStore
            .Setup(x => x.GetByRentalId(It.Is<int>(x => x == rentalId)))
            .Returns(bookings);

        bookingStore
            .Setup(x => x.Save(It.IsAny<Domain.Booking.Booking>()))
            .Returns(id);

        return bookingStore;
    }

    private static Mock<IMediator> GetMediatorMock(int rentalId, Mock<IBookingStore> bookingStoreMock)
    {
        var mediator = new Mock<IMediator>();

        var test = new Domain.Booking.Get.Many.Query(bookingStoreMock.Object);

        mediator
            .Setup(x => x.Send(It.IsAny<Domain.Booking.Get.Many.Request>(), It.IsAny<CancellationToken>()))
            .Returns(async () => await test.Handle(new Domain.Booking.Get.Many.Request(rentalId), CancellationToken.None));

        mediator
            .Setup(x => x.Send(It.IsAny<Domain.Rental.Get.Request>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new Domain.Rental.Get.Response(new Domain.Rental.Rental(rentalId, 1, 1))));

        return mediator;
    }

    public static IEnumerable<object[]> SuccessCreationData()
    {
        var rentalId = 1;

        var bookings = new List<Domain.Booking.Booking>();

        bookings.Add(new Domain.Booking.Booking
        {
            Id = 1,
            Nights = 1,
            Start = new DateOnly(2022, 2, 20),
            RentalId = rentalId
        });

        yield return new object[] { rentalId, bookings };


        bookings.Clear();
        bookings.Add(new Domain.Booking.Booking
        {
            Id = 1,
            Nights = 1,
            Start = new DateOnly(2022, 2, 26),
            RentalId = rentalId
        });

        yield return new object[] { rentalId, bookings };
    }
}
