using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RentalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<RentalViewModel> Get(int rentalId)
        {
            var result = await _mediator.Send(new Domain.Rental.Get.Request(rentalId));

            return new RentalViewModel
            {
                Id = result.Rental.Id,
                Units = result.Rental.Units
            };
        }

        [HttpPost]
        public async Task<ResourceIdViewModel> Post(RentalBindingModel model)
        {
            var query = new Domain.Rental.Create.Request(model.Units, model.PreparationTimeInDays);

            var result = await _mediator.Send(query);

            return new ResourceIdViewModel
            {
                Id = result.Id
            };
        }
    }
}
