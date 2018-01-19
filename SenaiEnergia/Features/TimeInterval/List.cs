using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SenaiEnergia.Domain;
using SenaiEnergia.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenaiEnergia.Features.TimeInterval
{
    public class List
    {
        public class Query : IRequest<Result>
        {
            public int CompanyId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(a => a.CompanyId).NotNull().NotEmpty().NotEqual(0).GreaterThan(0);
            }
            
        }



        public class Result
        {
            public List<TimeInterval> TimeIntervals { get; set; }

            public class TimeInterval
            {
                public TimeSpan StartTime { get; set; }
                public TimeSpan EndTime { get; set; }
                public decimal Value { get; set; }
                public string Type { get; set; }
            }

            
        }

        public class Handler : AsyncRequestHandler<Query, Result>
        {
            private readonly Db _db;

            public Handler(Db db)
            {
                _db = db;
            }

            protected override async Task<Result> HandleCore(Query message)
            {
                if (await _db.Companies.FindAsync(message.CompanyId) == null)
                {
                    throw new NotFoundException("Empresa não encontrada!");
                }

                var times = await _db.TimeIntervals.Where(a => a.CompanyId == message.CompanyId).Select(a => new Result.TimeInterval()
                {
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Value = a.Value,
                    Type = a.Type
                    
                }).ToListAsync();
                


                return new Result
                {
                    TimeIntervals = times
                };
            }
        }

    }
}
