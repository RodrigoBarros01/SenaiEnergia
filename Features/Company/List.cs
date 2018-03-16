using MediatR;
using Microsoft.EntityFrameworkCore;
using SenaiEnergia.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenaiEnergia.Features.Company
{
    public class List
    {
        public class Query : IRequest<Result>
        {

        }

        public class Result
        {
            public List<Company> Companies { get; set; }

            public class Company
            {
                public int Id { get; set; }
                public string Name { get; set; }
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
                var companies = await _db.Companies.Select(a => new Result.Company()
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToListAsync();

               
                return new Result
                {
                    Companies = companies
                };
            }
        }
    }
}
