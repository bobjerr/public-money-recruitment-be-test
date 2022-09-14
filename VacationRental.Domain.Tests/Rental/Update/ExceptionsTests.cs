using MediatR;
using Moq;
using VacationRental.Domain.Rental;

namespace VacationRental.Domain.Tests.Rental.Update;

public class ExceptionsTests
{
    [Theory]
    [InlineData(5, -1)]
    [InlineData(-5, 1)]
    public async Task Exception_by_incorrect_arguments(int units, int preparationTimeInDays)
    {
        var handler = new Domain.Rental.Update.Command(Mock.Of<IMediator>(), Mock.Of<IRentalRepository>(), new SemaphorService());

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() 
            => handler.Handle(new Domain.Rental.Update.Request(1, units, preparationTimeInDays), CancellationToken.None));
    }


    [Fact]
    public async Task Exception_not_found_rental()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => x.Send(It.IsAny<Domain.Rental.Get.Request>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ApplicationException());

        var handler = new Domain.Rental.Update.Command(mediatorMock.Object, Mock.Of<IRentalRepository>(), new SemaphorService());

        await Assert.ThrowsAsync<ApplicationException>(()
            => handler.Handle(new Domain.Rental.Update.Request(1, 1, 1), CancellationToken.None));
    }

    [Fact]
    public async Task Overlaps()
    {
        var mediatorMock = GetMediatorMock();

        var handler = new Domain.Rental.Update.Command(mediatorMock.Object, Mock.Of<IRentalRepository>(), new SemaphorService());

        await Assert.ThrowsAsync<ApplicationException>(()
            => handler.Handle(new Domain.Rental.Update.Request(1, 1, 1), CancellationToken.None));
    }

    private static Mock<IMediator> GetMediatorMock()
    {
        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(x => x.Send(It.IsAny<Domain.Rental.Get.Request>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new Domain.Rental.Get.Response(new Domain.Rental.Rental(1, 2, 3))));

        mediatorMock.Setup(x => x.Send(It.IsAny<Domain.Booking.Get.Many.Request>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new Domain.Booking.Get.Many.Response(new List<Domain.Booking.Booking>()
            {
                new Domain.Booking.Booking(1, 1, new DateOnly(2022, 1, 1), 1),
                new Domain.Booking.Booking(1, 1, new DateOnly(2022, 1, 1), 1),
            })));

        return mediatorMock;
    }
}
