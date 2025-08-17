using Dictionary.BusinessLogic.Translation.Requests;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoKrisMultilingualDictionary.Routes;

namespace MoKrisMultilingualDictionary.Controllers
{
    [ApiController]
    [Route(TranslationRoutes.ControllerBaseRoute)]
    public class TranslationController : ControllerBase
    {
        private readonly IMediator mediator;

        public TranslationController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> PostTranslation([FromBody] TranslationDto translation)
        {
            var request = new PostTranslationRequest()
            {
                SourceWordId = translation.SourceWordId,
                TargetWordId = translation.TargetWordId,
                LinkedTranslationGroupIds = translation.LinkedTranslationGroups
                                                .Select(ltg => ltg.TranslationGroupId)
                                                .ToList()
            };
            await mediator.Send(request);

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
