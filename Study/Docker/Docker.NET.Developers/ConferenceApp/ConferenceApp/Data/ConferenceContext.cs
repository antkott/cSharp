using ConferenceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConferenceApp.Data
{
    public class ConferenceContext : DbContext
    {
        public ConferenceContext(DbContextOptions<ConferenceContext> options)
            : base(options)
        {}

        public DbSet<Speaker> Speakers { get; set; }

        public DbSet<Sponsor> Sponsors { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    foreach (var entity in modelBuilder.Model.GetEntityTypes())
        //    {
        //        modelBuilder.Entity(entity.Name).ToTable(entity.ClrType.Name + "s");
        //    }
        //}
    }
}
