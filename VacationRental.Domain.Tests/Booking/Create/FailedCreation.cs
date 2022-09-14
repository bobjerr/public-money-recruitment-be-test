﻿using MediatR;
using Moq;
using VacationRental.Domain.Booking;
using VacationRental.Domain.Booking.Create;

namespace VacationRental.Domain.Tests.Booking.Create;

public class FailedCreation
{
    [Fact]
    public async Task Invalid_number_of_nights()
    {
        var handler = new Command(Mock.Of<IMediator>(), Mock.Of<IBookingRepository>(), new SemaphorService());

        var exception = await Assert.ThrowsAsync<ApplicationException>(async ()
            => await handler.Handle(new Request(5, new DateOnly(2022, 2, 20), -5), CancellationToken.None));

        Assert.Equal("Nights must be positive", exception.Message);
    }


    [Theory]
    [MemberData(nameof(SuccessCreationData))]
    public async Task Unsuccessfully_create_booking(int id, List<Domain.Booking.Booking> bookings)
    {
        int rentalId = 1;

        var bookingRepositoryMock = GetBookinRepositoryMock(id, rentalId, bookings);

        var mediatorMock = GetMediatorMock(rentalId, bookingRepositoryMock);

        var handler = new Command(mediatorMock.Object, bookingRepositoryMock.Object, new SemaphorService());

        var exception = await Assert.ThrowsAsync<ApplicationException>(async ()
            => await handler.Handle(new Request(5, new DateOnly(2022, 2, 20), 5), CancellationToken.None));

        Assert.Equal("Not available", exception.Message);
    }

    private static Mock<IBookingRepository> GetBookinRepositoryMock(int id, int rentalId, List<Domain.Booking.Booking> bookings)
    {
        var bookingRepository = new Mock<IBookingRepository>();

        bookingRepository
            .Setup(x => x.GetByRentalId(It.Is<int>(x => x == rentalId)))
            .Returns(bookings);

        bookingRepository
            .Setup(x => x.Save(It.IsAny<Domain.Booking.Booking>()))
            .Returns(id);

        return bookingRepository;
    }

    private static Mock<IMediator> GetMediatorMock(int rentalId, Mock<IBookingRepository> bookingRepositoryMock)
    {
        var mediator = new Mock<IMediator>();

        var query = new Domain.Booking.Get.Many.Query(bookingRepositoryMock.Object);

        mediator
            .Setup(x => x.Send(It.IsAny<Domain.Booking.Get.Many.Request>(), It.IsAny<CancellationToken>()))
            .Returns(async () => await query.Handle(new Domain.Booking.Get.Many.Request(rentalId), CancellationToken.None));

        mediator
            .Setup(x => x.Send(It.IsAny<Domain.Rental.Get.Request>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new Domain.Rental.Get.Response(new Domain.Rental.Rental(rentalId, 1, 1))));

        return mediator;
    }

    public static IEnumerable<object[]> SuccessCreationData()
    {
        var rentalId = 1;

        var bookings = new List<Domain.Booking.Booking>
        {
            new Domain.Booking.Booking(1, rentalId, new DateOnly(2022, 2, 20), 1)
        };

        yield return new object[] { rentalId, bookings };

        bookings.Clear();
        bookings.Add(new Domain.Booking.Booking(1, rentalId, new DateOnly(2022, 2, 26), 1));

        yield return new object[] { rentalId, bookings };
    }
}
