using MediatR;
using Moq;
using VacationRental.Domain.Booking;

namespace VacationRental.Domain.Tests.Booking.Create;

public class SuccessCreation
{
    [Theory]
    [MemberData(nameof(SuccessCreationData))]
    public async Task SuccessfullyCreatedBooking(int id, List<Domain.Booking.Booking> bookings)
    {
        int rentalId = 1;

        var bookingStoreMock = GetBookingStoreMock(id, rentalId, bookings);

        var mediatorMock = GetMediatorMock(rentalId, bookingStoreMock);

        var handler = new Domain.Booking.Create.Command(mediatorMock.Object, bookingStoreMock.Object);

        var response = await handler.Handle(new Domain.Booking.Create.Request(5, new DateOnly(2022, 2, 26), 4), CancellationToken.None);

        Assert.Equal(id, response.Id);
        bookingStoreMock.VerifyAll();
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
            .Returns(Task.FromResult(new Domain.Rental.Get.Response(new Domain.Rental.Rental(rentalId, 5, 1))));

        return mediator;
    }

    public static IEnumerable<object[]> SuccessCreationData()
    {
        var rentalId = 1;

        var bookings = new List<Domain.Booking.Booking>();

        yield return new object[] { rentalId, bookings };


        bookings.Add(
            new Domain.Booking.Booking
            {
                Id = 1,
                Nights = 2,
                Start = new DateOnly(2022, 1, 1),
                RentalId = rentalId
            });

        bookings.Add(
            new Domain.Booking.Booking
            {
                Id = 2,
                Nights = 4,
                Start = new DateOnly(2022, 2, 20),
                RentalId = rentalId
            });

        bookings.Add(
            new Domain.Booking.Booking
            {
                Id = 2,
                Nights = 4,
                Start = new DateOnly(2022, 2, 27),
                RentalId = rentalId
            }
        );

        yield return new object[] { rentalId, bookings };
    }
}
