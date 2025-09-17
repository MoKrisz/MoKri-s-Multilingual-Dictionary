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

        [HttpGet]
        public async Task<TranslationGroupDto> GetTranslationGroup([FromQuery] int translationGroupId)
        {
            var request = new GetTranslationGroupRequest { TranslationGroupId = translationGroupId };
            return await mediator.Send(request);
        }

        [HttpPost]
        public async Task<TranslationGroupDto> PostTranslationGroup([FromBody] TranslationGroupDto translationGroupDto)
        {
            var request = new PostTranslationGroupRequest { TranslationGroup = translationGroupDto };
            return await mediator.Send(request);
        }

        [HttpPut]
        public async Task<TranslationGroupDto> PutTranslationGroup([FromBody] TranslationGroupDto translationGroupDto)
        {
            var request = new PutTranslationGroupRequest { TranslationGroup = translationGroupDto };
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
