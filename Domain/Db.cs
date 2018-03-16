using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenaiEnergia.Domain
{
    public class Db : DbContext
    {
        public Db(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<TimeInterval> TimeIntervals { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder m)
        {
            base.OnModelCreating(m);
            m.Entity<Equipment>().ToTable(nameof(Equipment));
            m.Entity<TimeInterval>().ToTable(nameof(TimeInterval));
            m.Entity<Company>().ToTable(nameof(Company));
        }

    }
}
