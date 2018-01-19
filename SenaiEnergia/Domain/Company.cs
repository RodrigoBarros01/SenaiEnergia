using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SenaiEnergia.Domain
{
    public class Company
    {
        [Key]
        [Column("CompanyID")]
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<TimeInterval> TimeIntervals { get; set; }
    }
}
