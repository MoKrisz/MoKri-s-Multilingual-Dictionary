using Dictionary.BusinessLogic.Requests;
using Dictionary.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MoKrisMultilingualDictionary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordController : ControllerBase
    {
        private readonly IMediator mediator;

        public WordController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<WordDto> GetWord([FromQuery] int wordId)
        {
            var request = new GetWordRequest { WordId = wordId };
            return await mediator.Send(request);
        }
    }
}
