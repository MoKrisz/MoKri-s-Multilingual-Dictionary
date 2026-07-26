using Dictionary.BusinessLogic.Practice.GuessArticle.Requests;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoKrisMultilingualDictionary.Routes;

namespace MoKrisMultilingualDictionary.Controllers
{
    [ApiController]
    [Route(PracticeRoutes.ControllerBaseRoute)]
    public class PracticeController : ControllerBase
    {
        private readonly IMediator mediator;

        public PracticeController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet(PracticeRoutes.GetGuessArticleRandomWords)]
        public async Task<List<GuessArticleWordDto>> GetGuessArticleRandomWords([FromQuery] int languageCode, int amount)
        {
            var request = new GetGuessArticleRandomWordsRequest { LanguageCode = languageCode, Amount = amount };
            return await mediator.Send(request);
        }

        [HttpPost(PracticeRoutes.PostGuessArticleEvaluation)]
        public async Task<List<EvaluateGuessArticleResponseItemDto>> PostGuessArticleEvaluation([FromBody] EvaluateGuessArticleRequestDto requestDto)
        {
            var request = new EvaluateGuessArticleRequest() { Guesses = requestDto.Guesses };

            return await mediator.Send(request);
        }
    }
}
