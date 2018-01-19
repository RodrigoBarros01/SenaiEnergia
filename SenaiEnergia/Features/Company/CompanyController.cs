using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SenaiEnergia.Features.Company
{
    [Produces("application/json")]
    [Route("api")]
    public class CompanyController : Controller
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("companies")]
        public async Task<IActionResult> List(List.Query query)
        {
            var model = await _mediator.Send(query);

            return Ok(model);
        }



    }
}