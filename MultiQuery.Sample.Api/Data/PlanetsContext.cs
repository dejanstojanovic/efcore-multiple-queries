using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MultiQuery.Sample.Api.Data
{
    public class PlanetsContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        public PlanetsContext(
            DbContextOptions<PlanetsContext> options,
            ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Planet>(new PlanetsConfiguration());
        }

        public DbSet<Planet> Planets { get; set; }

    }
}
