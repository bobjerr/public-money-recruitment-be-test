using Moq;
using VacationRental.Domain.Rental;

namespace VacationRental.Domain.Tests.Rental.Create
{
    public class Tests
    {
        [Theory]
        [InlineData(5, 3, 4)]
        public async Task Successfully_Rental_Creation(int units, int preparationTimeInDays, int id)
        {
            var rentalRepository = new Mock<IRentalRepository>();

            rentalRepository
                .Setup(x => x.Create(It.Is<int>(x => x == units), It.Is<int>(x => x == preparationTimeInDays)))
                .Returns(id);

            var handler = new Domain.Rental.Create.Command(rentalRepository.Object);

            var response = await handler.Handle(new Domain.Rental.Create.Request(units, preparationTimeInDays), CancellationToken.None);

            Assert.Equal(id, response.Id);
            rentalRepository.VerifyAll();
        }
    }
}