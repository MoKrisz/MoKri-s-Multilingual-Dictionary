using Dictionary.BusinessLogic.Requests;
using Dictionary.BusinessLogic.TranslationGroup.Requests;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace MoKrisMultilingualDictionary.Controllers
{
    public class TranslationGroupListController : ODataController
    {
        private readonly IMediator mediator;

        public TranslationGroupListController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [EnableQuery]
        public async Task<List<TranslationGroupDto>> Get()
        {
            var request = new GetTranslationGroupListRequest();
            return await mediator.Send(request);
        }
    }
}
