using MediatR;
using Microsoft.AspNetCore.Mvc;
using SNM.BrandManagement.Application.DTO;
using SNM.BrandManagement.Application.Exceptions.Model;
using SNM.BrandManagement.Application.Features.Commands.ChannelProfiles;
using SNM.BrandManagement.Application.Features.Commands.Channels;
using SNM.BrandManagement.Application.Features.Queries.ChannelProfiles;
using SNM.BrandManagement.Application.Features.Queries.Channels;
using SNM.BrandManagement.Application.Features.Queries.ChannelTypes;
using SNM.BrandManagement.Domain.Entities;
using SNM.BrandManagement.Domain.Enumeration;
using System.Net;
using System.Xml.Linq;

namespace SNM.BrandManagement.Presentation.Controllers
{
    [ApiController]
    // [Route("api/[controller]")]
    [Route("api/v1/[controller]")]
    public class ChannelProfileController : Controller
    {

        private readonly IMediator _mediator;
        public ChannelProfileController(IMediator mediator)
            {
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            }



            //[HttpGet("{id}", Name = "GetById")]
            [HttpGet("{id:guid}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesDefaultResponseType]
            public async Task<ActionResult> GetChannelProfileByBrandIdQuery(Guid id)
            {
                var getEntity = new GetChannelProfileByBrandIdQuery(id);
                var entity = await _mediator.Send(getEntity);

                return Ok(entity);
            }



            [HttpPost( "CreateChannelProfile")]
        [ProducesDefaultResponseType]
        public async Task<ChannelProfile> CreateChannelProfile([FromBody] ChannelProfileDto command, [FromQuery] string ChanelTypeName, [FromQuery] Guid brandId)
            {
            Response<ChannelProfile> channelprofile = new Response<ChannelProfile>();
          
            List<Channel> channelbyProfileId = new List<Channel>();
            switch (ChanelTypeName)
            {
                case "LinkedIn Page":
                    {
                        Guid channelTypeIdForSocialChannels = (await _mediator.Send(new GetChannelTypesByNameQuery(ChanelTypeName))).Id;
                        command.Channel.ToList().ForEach(item => item.ChannelTypeId = channelTypeIdForSocialChannels);
                        var result = await _mediator.Send(new CreateChannelProfileCommand { ChannelProfileDto = command });
                        Guid channelTypeId = (await _mediator.Send(new GetChannelTypesByNameQuery("LinkedIn Profile"))).Id;

                        CreateChannelCommand channel = new CreateChannelCommand
                        {
                            DisplayName = command.UserName,
                            Photo = command.CoverPhoto,
                            BrandId = brandId,
                            ChannelProfileId = result.Id,
                            ChannelTypeId = channelTypeId,
                            IsActivated = Domain.Enumeration.ActivationStatus.Active,
                            Link = command.ProfileLink,
                            SocialChannelId=command.ProfileUserId,
                           
                            
                        };
                       await _mediator.Send(channel);
                         //channelbyProfileId=   await _mediator.Send(new GetChannelByChannelProfileId(result.Id));
                        //result.Channels.Add(channelforProfile);
                   
                        //result.SocialChannels.Add(channelforProfile);
                        channelprofile.Succeeded = true;
                        channelprofile.Data = result;
                        
                        break;
                    }
                case "Twitter Profile":
                    {
                        Guid channelTypeIdForSocialChannels = (await _mediator.Send(new GetChannelTypesByNameQuery(ChanelTypeName))).Id;
                        command.Channel.FirstOrDefault().ChannelTypeId = channelTypeIdForSocialChannels;
                        command.Channel.FirstOrDefault().IsActivated= Domain.Enumeration.ActivationStatus.Active;   
                        var result = await _mediator.Send(new CreateChannelProfileCommand { ChannelProfileDto = command });
                        channelprofile.Succeeded = true;
                        channelprofile.Data = result;
                        break;
                    }
                case "Instagram Profile":
                    {
                        Guid channelTypeIdForSocialChannels = (await _mediator.Send(new GetChannelTypesByNameQuery(ChanelTypeName))).Id;
                        command.Channel.ToList().ForEach(item => { item.ChannelTypeId = channelTypeIdForSocialChannels;
                            item.BrandId = brandId;
                        });
                        var result = await _mediator.Send(new CreateChannelProfileCommand { ChannelProfileDto = command });
                        channelprofile.Data = result;
                      //  Guid channelTypeId = (await _mediator.Send(new GetChannelTypesByNameQuery("Instagram Profile"))).Id;
                       
                        //CreateChannelCommand channel = new CreateChannelCommand
                        //{
                        //    DisplayName = command.UserName,
                        //    Photo = command.CoverPhoto,
                        //    BrandId = brandId,
                        //    ChannelProfileId = result.Id,
                        //    ChannelTypeId = channelTypeId,
                        // //   IsActivated = Domain.Enumeration.ActivationStatus.Active,

                        //};
                        //await _mediator.Send(channel);

                        channelprofile.Succeeded = true;
                        
                   
                        break;
                    }
                case "Facebook Page":
                    {

                        Guid channelTypeIdForSocialChannels = (await _mediator.Send(new GetChannelTypesByNameQuery(ChanelTypeName))).Id;
                        command.Channel.ToList().ForEach(item =>
                        {
                            item.ChannelTypeId = channelTypeIdForSocialChannels;
                            item.BrandId = brandId;
                        });
                        var result = await _mediator.Send(new CreateChannelProfileCommand { ChannelProfileDto = command });
                        //Guid channelTypeId = (await _mediator.Send(new GetChannelTypesByNameQuery("Facebook Profile"))).Id;
                        //CreateChannelCommand channel = new CreateChannelCommand
                        //{
                        //    DisplayName = command.UserName,
                        //    Photo = command.CoverPhoto,
                        //    BrandId = brandId,
                        //    ChannelProfileId = result.Id,
                        //    ChannelTypeId = channelTypeId,
                        //    IsActivated = Domain.Enumeration.ActivationStatus.Active,

                        //};
                        //await _mediator.Send(channel);
                        channelprofile.Succeeded = true;
                        channelprofile.Data = result;
                        break;
                    }
                case "Facebook Group":
                    {

                        Guid channelTypeIdForSocialChannels = (await _mediator.Send(new GetChannelTypesByNameQuery(ChanelTypeName))).Id;
                        command.Channel.ToList().ForEach(item =>
                        {
                            item.ChannelTypeId = channelTypeIdForSocialChannels;
                            item.BrandId = brandId;
                        });
                        var result = await _mediator.Send(new CreateChannelProfileCommand { ChannelProfileDto = command });
                        //Guid channelTypeId = (await _mediator.Send(new GetChannelTypesByNameQuery("Facebook Profile"))).Id;
                        //CreateChannelCommand channel = new CreateChannelCommand
                        //{
                        //    DisplayName = command.UserName,
                        //    Photo = command.CoverPhoto,
                        //    BrandId = brandId,
                        //    ChannelProfileId = result.Id,
                        //    ChannelTypeId = channelTypeId,
                        //    IsActivated = Domain.Enumeration.ActivationStatus.Active,

                        //};
                        //await _mediator.Send(channel);
                        channelprofile.Succeeded = true;
                        channelprofile.Data = result;
                        break;
                    }
            }



         
            return channelprofile.Data;
            }

            //[HttpPut(Name = "Update")]
            [HttpPut, Route("[controller]/UpdateChannelProfile")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesDefaultResponseType]
            public async Task<ActionResult> Update([FromBody] UpdateChannelProfileCommand command)
            {
                await _mediator.Send(command);
                return NoContent();
            }


            [HttpDelete, Route("[controller]/Delete")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesDefaultResponseType]
            public async Task<ActionResult> Delete(Guid id)
            {
                var command = new DeleteProfileChannelCommand() { Id = id };
                await _mediator.Send(command);
                return NoContent();
            }
        }
    }

