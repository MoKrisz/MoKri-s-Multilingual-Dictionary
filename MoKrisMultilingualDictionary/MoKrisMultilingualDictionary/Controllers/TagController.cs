using Dictionary.BusinessLogic.Requests;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoKrisMultilingualDictionary.Routes;

namespace MoKrisMultilingualDictionary.Controllers
{
    [ApiController]
    [Route(TagRoutes.ControllerBaseRoute)]
    public class TagController : ControllerBase
    {
        private readonly IMediator mediator;

        public TagController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet(TagRoutes.GetClosestMatchingTagRoute)]
        public async Task<TagDto?> GetClosestMatchingTag([FromQuery] string tagText)
        {
            var request = new GetClosestMatchingTagRequest { TagText = tagText };
            return await mediator.Send(request);
        }
    }
}
