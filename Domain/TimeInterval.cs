using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SenaiEnergia.Domain
{
    public class TimeInterval
    {
        [Key]
        [Column("TimeIntervalID")]
        public int Id { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public decimal Value { get; set; }

        public string Type { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
