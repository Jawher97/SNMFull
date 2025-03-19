using SNM.BrandManagement.Application.Exceptions.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SNM.BrandManagement.Application.Features.Queries.Brand.GetBrands;
using SNM.BrandManagement.Application.Features.Commands.Brands.CreateBrand;
using SNM.BrandManagement.Application.Features.Commands.Brands.UpdateBrand;
using SNM.BrandManagement.Application.Features.Commands.Brands.DeleteBrand;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using SNM.BrandManagement.Application.Features.Queries.Brand;

namespace SNM.BrandManagement.Presentation.Controllers
{

    [ApiController]
    // [Route("api/[controller]")]
    [Route("api/v1/[controller]/[action]")]
    public class BrandController : Controller
    {
        private readonly IMediator _mediator;

        public BrandController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet(Name = "GetBrand")]
        [ProducesResponseType(typeof(IEnumerable<GetBrandsViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<GetBrandsViewModel>>> GetAll()
        {
            var getEntities = new GetBrandsQuery();
            var entities = await _mediator.Send(getEntities);
            return Ok(entities);
        }

        [HttpGet(Name = "GetBrandsAndChannels")]
        [ProducesResponseType(typeof(IEnumerable<GetBrandsViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<GetBrandsViewModel>>> GetBrandsAndChannels()
        {
            var getEntities = new GetBrandsAndChannelsQuery();
            var entities = await _mediator.Send(getEntities);
            return Ok(entities);
        }
       


        [HttpGet("{id}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetById(Guid id)
        {
            var getEntity = new GetBrandByIdQuery(id);
            var entity = await _mediator.Send(getEntity);

            return Ok(entity);
        }

        [HttpPost(Name = "CreateBrand")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> CreateBrand([FromForm] CreateBrandCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new Response<Guid>(result, $"Successfully created with Id: {result}"));
        }

        [HttpPatch(Name = "UpdateBrand")]
        //[HttpPut, Route("[controller]/Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateBrand([FromBody] UpdateBrandCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete(Name = "DeleteBrand")]
        //[HttpDelete("{id:guid}")]
        //[HttpDelete, Route("[controller]/DeleteBrand")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteBrand( [FromQuery] Guid id)
        {
            var command = new DeleteBrandCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpPost, DisableRequestSizeLimit]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Upload()
        {
            try
            {
                var files = Request.Form.Files;
                var folderName = Path.Combine("StaticFiles", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (files.Any(f => f.Length == 0))
                {
                    return BadRequest();
                }

                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName); //you can add this path to a list and then return all dbPaths to the client if require

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok("All the files are successfully uploaded.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
