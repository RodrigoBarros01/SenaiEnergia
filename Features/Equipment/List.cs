using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SenaiEnergia.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenaiEnergia.Features.Equipment
{
    public class List
    {
        public class Query : IRequest<Result>
        {

        }

        public class Result
        {

            public List<Equipment> Equipments { get; set; }

            public class Equipment
            {
                public string Name { get; set; }
                public double EletricPower { get; set; }
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
                var equipments = await _db.Equipments.Select(a => new Result.Equipment()
                {
                    EletricPower = a.EletricPower,
                    Name = a.Name
                }).ToListAsync();
                

                return new Result
                {
                    Equipments = equipments
                };
            }

        }


    }
}
