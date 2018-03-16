using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SenaiEnergia.Features.TimeInterval
{
    [Produces("application/json")]
    [Route("api")]
    public class TimeIntervalController : Controller
    {
        private readonly IMediator _mediator;

        public TimeIntervalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpGet]
        [Route("timeintervals/{companyId}")]
        public async Task<IActionResult> List(List.Query query)
        {
            var model = await _mediator.Send(query);

            return Ok(model);
        }


    }
}