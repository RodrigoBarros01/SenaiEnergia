using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SenaiEnergia.Features.Calculation
{
    [Produces("application/json")]
    [Route("api")]
    public class CalculateController : Controller
    {
        private readonly IMediator _mediator;

        public CalculateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("calculate")]
        public async Task<IActionResult> Calculate([FromBody]Calculate.Command command)
        {
            var result = await _mediator.Send(command);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }


    }
}