using MediatR;
using Microsoft.AspNetCore.Mvc;
using SNM.BrandManagement.Application.Features.Queries.ChannelTypes;

namespace SNM.BrandManagement.Presentation.Controllers
{
     [Route("api/v1/[controller]")]
    public class ChannelTypeController : Controller
    {
        private readonly IMediator _mediator;

        public ChannelTypeController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("GetChannelTypesByBrandId")]
        //[HttpGet("{brandId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetChannelTypesByBrandId(Guid brandId)
        {
            var getEntities = new GetChannelTypesByBrandIdQuery(brandId);
            var entities = await _mediator.Send(getEntities);

            return Ok(entities);
        }
        [HttpGet("GetChannelTypesByNameId")]
        //[HttpGet("{brandId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetChannelTypesByName([FromQuery] string Name)
        {
            var getEntities = new GetChannelTypesByNameQuery(Name);
            var entities = await _mediator.Send(getEntities);

            return Ok(entities);
        }
       
    }
}
