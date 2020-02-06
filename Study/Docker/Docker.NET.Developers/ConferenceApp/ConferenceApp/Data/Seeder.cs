using ConferenceApp.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConferenceApp.Data
{
    public static class Seeder
    {
        public static void SeedDatabase(IServiceProvider serviceProvider)
        {
            var speakers = new List<Speaker>
            {
                new Speaker
                {
                    First = "Jim",
                    Last = "Beam",
                    EmailAddress = "jim.beam@bourbon.com"
                },
                new Speaker
                {
                    First = "Jack",
                    Last = "Daniels",
                    EmailAddress = "jack.daniels@whisky.com"
                }
            };

            var sponsors = new List<Sponsor>
            {
                new Sponsor
                {
                    CompanyName = "Bad Mutha Distillery",
                    Level = "Keg",
                    Description = "A really cool distillery.",
                    Url = "https://this.doesnot.exist/"
                }
            };

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ConferenceContext>();

                if (!context.Speakers.Any())
                {
                    context.Speakers.AddRange(speakers);
                    context.SaveChanges();
                }

                if (!context.Sponsors.Any())
                {
                    context.Sponsors.AddRange(sponsors);
                    context.SaveChanges();
                }
            }
        }
    }
}
