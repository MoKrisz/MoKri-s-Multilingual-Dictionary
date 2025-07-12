using Dictionary.BusinessLogic.TranslationGroup.Requests;
using Dictionary.Domain;
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

        [HttpPost]
        public async Task<TranslationGroupDto> PostTranslationGroup([FromBody] TranslationGroupDto translationGroupDto)
        {
            var request = new PostTranslationGroupRequest { TranslationGroup = translationGroupDto };
            return await mediator.Send(request);
        }

        [HttpGet(TranslationGroupRoutes.WordRelatedTranslationGroupsRoute)]
        public async Task<WordRelatedTranslationGroupsDto> GetWordRelatedTranslationGroups([FromQuery] int sourceWordId, int targetWordId)
        {
            var request = new GetWordRelatedTranslationGroupsRequest 
            { 
                SourceWordId = sourceWordId,
                TargetWordId = targetWordId
            };
            return await mediator.Send(request);
        }
    }
}
