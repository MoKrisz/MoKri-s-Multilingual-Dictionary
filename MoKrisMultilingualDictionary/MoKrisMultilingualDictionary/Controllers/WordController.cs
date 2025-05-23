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

        public const string PostWordRoute = "word";
        [HttpPost(PostWordRoute)]
        public async Task<int> PostWord([FromBody] WordDto wordDto)
        {
            var request = new PostWordRequest { Word = wordDto };
            return await mediator.Send(request);
        }

        public const string PutWordRoute = "word";
        [HttpPut(PutWordRoute)]
        public async Task<IActionResult> PutWord([FromBody] WordDto wordDto)
        {
            var request = new PutWordRequest { Word = wordDto };
            await mediator.Send(request);
            return Ok();
        }
    }
}
