using System.Linq;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Web.IntegrationTests.Database.StandingData;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Database
{
    internal class StandingDataLoad
    {
        internal static void Load(MatchingDbContext context)
        {
            context.AddRange(EmployerLoad.Create().ToList());
            context.AddRange(RouteLoad.Create().ToList());

            context.SaveChanges();
        }
    }
}