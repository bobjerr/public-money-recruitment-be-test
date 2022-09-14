using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Booking;
using VacationRental.Domain.Rental;

namespace VacationRental.Domain.Tests.Rental.Update;

public class SuccessTests
{
    [Fact]
    public async Task Exception_not_found_rental()
    {
        var rentalMock = new Mock<IRentalRepository>();

        var mediatorMock = GetMediatorMock();

        var handler = new Domain.Rental.Update.Command(mediatorMock.Object, rentalMock.Object, new SemaphorService());

        var response = await handler.Handle(new Domain.Rental.Update.Request(1, 1, 1), CancellationToken.None);

        rentalMock.Verify(x => x.Update(It.Is<Domain.Rental.Rental>(r => r.Units == 1 && r.PreparationTimeInDays == 1)));
    }

    private static Mock<IMediator> GetMediatorMock()
    {
        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(x => x.Send(It.IsAny<Domain.Rental.Get.Request>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new Domain.Rental.Get.Response(new Domain.Rental.Rental(1, 2, 3))));

        mediatorMock.Setup(x => x.Send(It.IsAny<Domain.Booking.Get.Many.Request>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new Domain.Booking.Get.Many.Response(new List<Domain.Booking.Booking>())));
        return mediatorMock;
    }
}
