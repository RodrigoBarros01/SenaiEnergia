using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SenaiEnergia.Domain;

namespace SenaiEnergia.Features.Equipment
{
    public class EquipmentController : Controller
    {
        private readonly IMediator _mediator;

        public EquipmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("equipments")]
        public async Task<IActionResult> List(List.Query query)
        {
            var model = await _mediator.Send(query);

            return Ok(model);
        }
    }
}