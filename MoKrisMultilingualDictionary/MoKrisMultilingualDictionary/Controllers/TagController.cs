using Dictionary.BusinessLogic.Requests;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MoKrisMultilingualDictionary.Controllers
{
    [ApiController]
    [Route("tag")]
    public class TagController : ControllerBase
    {
        private readonly IMediator mediator;

        public TagController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public const string GetClosestMatchingTagRoute = "closest-matching-tag";
        [HttpGet(GetClosestMatchingTagRoute)]
        public async Task<TagDto?> GetClosestMatchingTag([FromQuery] string tagText)
        {
            var request = new GetClosestMatchingTagRequest { TagText = tagText };
            return await mediator.Send(request);
        }
    }
}
