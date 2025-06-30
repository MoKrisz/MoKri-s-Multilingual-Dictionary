using Dictionary.BusinessLogic.TranslationGroup.Requests;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoKrisMultilingualDictionary.Routes;

namespace MoKrisMultilingualDictionary.Controllers
{
    [ApiController]
    [Route(TranslationGroupRoutes.ControllerBaseRoute)]
    public class TranslationGroupController : ControllerBase
    {
        private readonly IMediator mediator;

        public TranslationGroupController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        //public const string GetTranslationGroupRoute = "translation-group";
        //[HttpGet(GetTranslationGroupRoute)]
        //public async Task<TranslationGroupDto> GetTranslationGroup([FromQuery] int translationGroupId)
        //{
        //    var request = new GetTranslationGroupRequest { TranslationGroupId = translationGroupId };
        //    return await mediator.Send(request);
        //}

        [HttpPost]
        public async Task<int> PostTranslationGroup([FromBody] TranslationGroupDto translationGroupDto)
        {
            var request = new PostTranslationGroupRequest { TranslationGroup = translationGroupDto };
            return await mediator.Send(request);
        }

        //public const string PutTranslationGroupRoute = "translation-group";
        //[HttpPut(PutTranslationGroupRoute)]
        //public async Task<IActionResult> PutTranslationGroup([FromBody] TranslationGroupDto translationGroupDto)
        //{
        //    var request = new PutTranslationGroupRequest { TranslationGroup = translationGroupDto };
        //    await mediator.Send(request);
        //    return Ok();
        //}
    }
}
