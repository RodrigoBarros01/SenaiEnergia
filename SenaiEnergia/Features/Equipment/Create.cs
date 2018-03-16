
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SenaiEnergia.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenaiEnergia.Features.Equipment
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Name { get; set; }
            public double EletricPower { get; set; }
        }


        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(e => e.Name).NotNull();
                RuleFor(e => e.EletricPower).NotNull().GreaterThan(0);
            }
        }

        public class Handler : RequestHandler<Command>
        {
            private readonly Db _db;

            public Handler(Db db)
            {
                _db = db;
            }

            protected override void HandleCore(Command message)
            {
                var equipment = new Domain.Equipment {
                    Name = message.Name,
                    EletricPower = message.EletricPower
                };

                _db.Equipments.Add(equipment);
                _db.SaveChanges();
            }
            
        }



    }
}
