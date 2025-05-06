using Dictionary.BusinessLogic.Requests;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace MoKrisMultilingualDictionary.Controllers
{
    public class WordListController : ODataController
    {
        private readonly IMediator mediator;

        public WordListController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [EnableQuery]
        public async Task<List<WordDto>> Get()
        {
            var request = new GetWordsRequest();
            return await mediator.Send(request);
        }
    }
}
