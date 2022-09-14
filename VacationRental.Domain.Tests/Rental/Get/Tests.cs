using Moq;
using VacationRental.Domain.Rental;

namespace VacationRental.Domain.Tests.Rental.Get;

public class Exceptions
{
    [Theory]
    [InlineData(5)]
    public async Task Successfully_get_rentals(int id)
    {
        var rental = new Domain.Rental.Rental(id, 5, 8);
        var rentalRepository = new Mock<IRentalRepository>();

        rentalRepository
            .Setup(x => x.Get(It.Is<int>(x => x == id)))
            .Returns(rental);

        var handler = new Domain.Rental.Get.Query(rentalRepository.Object);

        var response = await handler.Handle(new Domain.Rental.Get.Request(id), CancellationToken.None);

        Assert.Equal(rental.Id, response.Rental.Id);
        Assert.Equal(rental.Units, response.Rental.Units);
        Assert.Equal(rental.PreparationTimeInDays, response.Rental.PreparationTimeInDays);
    }

    [Theory]
    [InlineData(5)]
    public async Task Unsuccessfully_get_rentals(int id)
    {
        var rental = new Domain.Rental.Rental(id, 5, 8);
        var rentalRepository = new Mock<IRentalRepository>();

        rentalRepository
            .Setup(x => x.Get(It.Is<int>(x => x == id)))
            .Throws(() => new ApplicationException());

        var handler = new Domain.Rental.Get.Query(rentalRepository.Object);

        await Assert.ThrowsAsync<ApplicationException>(async () => await handler.Handle(new Domain.Rental.Get.Request(id), CancellationToken.None));
    }
}
