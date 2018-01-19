
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SenaiEnergia.Domain;
using SenaiEnergia.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenaiEnergia.Features.Calculation
{
    public class Calculate
    {
        public class Command : IRequest<Result>
        {
            public double Power { get; set; }
            public int Quantity { get; set; }
            public int Time { get; set; }
            public TimeSpan StartUse { get; set; } //validar quando isso tiver nulo (ta vindo 00:00:00)
            public int DaysOfUse { get; set; }
            public int CompanyId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(a => a.Power).NotNull().NotEmpty().NotEqual(0).GreaterThan(0);
                RuleFor(a => a.Quantity).NotNull().NotEmpty().NotEqual(0).GreaterThan(0);
                RuleFor(a => a.Time).NotNull().NotEmpty().NotEqual(0).GreaterThan(0).LessThanOrEqualTo(1440);
                RuleFor(a => a.DaysOfUse).NotNull().NotEmpty().NotEqual(0).GreaterThan(0).LessThanOrEqualTo(31);
                RuleFor(a => a.CompanyId).NotNull().NotEmpty().NotEqual(0).GreaterThan(0);

                RuleFor(a => a.StartUse).NotNull().LessThanOrEqualTo(new TimeSpan(23, 59, 59)).GreaterThanOrEqualTo(new TimeSpan(0, 0, 0));
                
            }
        }


        public class Result
        {
            public decimal ConventionalRateMonth { get; set; }
            public decimal WhiteRateMonth { get; set; }
            public TimeSpan ComputationTime { get; set; }
        }


        //public class Handler : AsyncRequestHandler<Command, Result>
        //{
        //    private readonly Db _db;

        //    public Handler(Db db)
        //    {
        //        _db = db;
        //    }

        //    protected override async Task<Result> HandleCore(Command message)
        //    //public async Task<Result> Handle(Command message)
        //    {
        //        #region Calcula tempo final de uso do equipamento

        //        TimeSpan endUse = message.StartUse.Add(new TimeSpan(0, message.Time, 0));

        //        #endregion

        //        #region Popula lista de TempoValor para calculo

        //        List<TimeValue> timeValues = new List<TimeValue>();

        //        TimeSpan validaHorario = new TimeSpan(23, 59, 59);

        //        List<StartEnd> times = new List<StartEnd>();

        //        if (validaHorario < endUse)
        //        {
        //            times.Add(new StartEnd
        //            {
        //                Start = message.StartUse,
        //                End = new TimeSpan(23, 59, 0)
        //            });
        //            times.Add(new StartEnd
        //            {
        //                Start = new TimeSpan(0, 0, 0),
        //                End = endUse.Subtract(new TimeSpan(24, 0, 0))
        //            });
        //        }
        //        else
        //        {
        //            times.Add(new StartEnd
        //            {
        //                Start = message.StartUse,
        //                End = endUse
        //            });
        //        }

        //        foreach (var time in times)
        //        {
        //            if (_db.TimeIntervals.Any(a => a.StartTime <= time.Start && a.EndTime >= time.End && a.Company.Id == message.CompanyId))
        //            {
        //                var timeInterval = _db.TimeIntervals.Where(a => a.StartTime <= time.Start && a.EndTime >= time.End && a.Company.Id == message.CompanyId).FirstOrDefault();
        //                timeValues.Add(new TimeValue
        //                {
        //                    TimeInMinutes = (int)(time.End - time.Start).TotalMinutes,
        //                    Value = timeInterval.Value
        //                });
        //            }
        //            else
        //            {
        //                double totalTime = (time.End - time.Start).TotalMinutes;
        //                double variableTime = 0;
        //                var start = time.Start;
        //                var end = time.End;

        //                while (totalTime > 0)
        //                {
        //                    if (_db.TimeIntervals.Any(a => a.StartTime <= start && a.EndTime >= start && a.EndTime <= end && a.Company.Id == message.CompanyId && a.Type == "Branca"))
        //                    {
        //                        var timeInterval = _db.TimeIntervals.Where(a => a.StartTime <= start && a.EndTime >= start && a.EndTime <= end && a.Company.Id == message.CompanyId).FirstOrDefault();
        //                        variableTime = (timeInterval.EndTime - start).TotalMinutes;

        //                        totalTime -= variableTime;
        //                        start = start.Add(new TimeSpan(0, (int)variableTime, 0));


        //                        timeValues.Add(new TimeValue
        //                        {
        //                            TimeInMinutes = (int)variableTime,
        //                            Value = timeInterval.Value
        //                        });
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }
        //            }
        //        }

        //        #endregion


        //        #region Calcula

        //        List<KhwDayValue> khwDayValues = new List<KhwDayValue>();

        //        foreach (var timeValue in timeValues)
        //        {
        //            var calcKwh = ((decimal)message.Quantity * (decimal)message.Power * ((decimal)timeValue.TimeInMinutes / 60) / 1000);
        //            khwDayValues.Add(new KhwDayValue
        //            {
        //                KhwDay = calcKwh,
        //                Value = timeValue.Value
        //            });

        //        }

        //        decimal white = 0;
        //        decimal lowerWhite = _db.TimeIntervals.Where(a => a.Company.Id == message.CompanyId && a.Type == "Branca").Min(a => a.Value);
        //        decimal conventional = 0;
        //        var conventionalRateValue = (await _db.TimeIntervals.Where(a => a.Company.Id == message.CompanyId && a.Type == "Convencional").FirstOrDefaultAsync()).Value;


        //        foreach (var item in khwDayValues)
        //        {
        //            white += ((item.KhwDay * (item.Value / 1000) * 22) + (item.KhwDay * (lowerWhite / 1000) * 8));
        //            conventional += (item.KhwDay * (conventionalRateValue / 1000) * 30);
        //        }

        //        decimal percentual = (decimal)message.DaysOfUse / 30;

        //        return new Result //validar resultado depois de percentual adicionado
        //        {
        //            WhiteRateMonth = white * percentual,
        //            ConventionalRateMonth = conventional * percentual
        //        };


        //        #endregion

        //    }

        //    #region Classes de apoio para o calculo

        //    public class TimeValue
        //    {
        //        public int TimeInMinutes { get; set; }
        //        public decimal Value { get; set; }
        //    }

        //    public class KhwDayValue
        //    {
        //        public decimal KhwDay { get; set; }
        //        public decimal Value { get; set; }
        //    }

        //    public class StartEnd
        //    {
        //        public TimeSpan Start { get; set; }
        //        public TimeSpan End { get; set; }
        //    }

        //    #endregion



        //}

        public class Handler : AsyncRequestHandler<Command, Result>
        {
            private readonly Db _db;
            private readonly IConfiguration configuration;


            public Handler(Db db, IStartup startup, IConfiguration configuration)
            {
                _db = db;
                if (startup is Startup s)
                    this.configuration = s.Configuration;
                else this.configuration = configuration;
            }

            protected override async Task<Result> HandleCore(Command message)
            {

                if (await _db.Companies.FindAsync(message.CompanyId) == null)
                {
                    throw new NotFoundException("Empresa não encontrada!");
                }

                DateTime start = DateTime.Now;
                TimeSpan endUse = message.StartUse.Add(new TimeSpan(0, message.Time, 0));

                DateTime cursor = new DateTime(1, 1, 1).Add(message.StartUse);
                DateTime endDate = cursor.AddMinutes(message.Time);

                Result result = new Result();

                var companyTimes = await _db.TimeIntervals.Where(d => d.CompanyId == message.CompanyId).ToArrayAsync();
                var conventionalValue = (companyTimes.FirstOrDefault(a => a.Type == "Convencional"))?.Value ?? 0m;
                var weekendValue = (companyTimes.FirstOrDefault(a => a.Type == "FinalDeSemana"))?.Value ?? 0m;
                while (cursor < endDate)
                {
                    var timeInterval = companyTimes.FirstOrDefault(a => CheckTimeCollision(a, cursor));
                    if (timeInterval != null)
                    {
                        //result.WhiteRateMonth += (message.Quantity * (decimal)message.Power * (1 / 60m) / 1000m) * (timeInterval.Value / 1000m) * message.DaysOfUse;
                        result.WhiteRateMonth += (message.Quantity * (decimal)message.Power * ((1 / 60m) / 1000m) * (timeInterval.Value / 1000m) * 22) + ((message.Quantity * (decimal)message.Power * (1 / 60m) / 1000m) * (weekendValue / 1000m) * 8);

                        result.ConventionalRateMonth += (message.Quantity * (decimal)message.Power * (1 / 60m) / 1000m) * (conventionalValue / 1000m) * message.DaysOfUse;
                    }

                    cursor = cursor.AddMinutes(1);
                }
                result.ComputationTime = DateTime.Now - start;
                result.WhiteRateMonth = result.WhiteRateMonth * message.DaysOfUse / 30;
                return result;
            }

            private static bool CheckTimeCollision(Domain.TimeInterval a, DateTime cursor) =>
                    a.StartTime <= cursor.TimeOfDay && cursor.TimeOfDay <= a.EndTime && a.Type == "Branca";
        }
    }
}
