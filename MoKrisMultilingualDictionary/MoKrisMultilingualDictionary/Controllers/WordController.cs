using Dictionary.BusinessLogic.Requests;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MoKrisMultilingualDictionary.Controllers
{
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IMediator mediator;

        public WordController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public const string GetWordRoute = "word";
        [HttpGet(GetWordRoute)]
        public async Task<WordDto> GetWord([FromQuery] int wordId)
        {
            var request = new GetWordRequest { WordId = wordId };
            return await mediator.Send(request);
        }

        public const string GetWordsRoute = "words";
        [HttpGet(GetWordsRoute)]
        public async Task<List<WordDto>> GetWords()
        {
            var request = new GetWordsRequest();
            return await mediator.Send(request);
        }
    }
}
