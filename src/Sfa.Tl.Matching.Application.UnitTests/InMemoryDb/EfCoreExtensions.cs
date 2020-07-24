using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Sfa.Tl.Matching.Application.UnitTests.InMemoryDb
{
    public static class EfCoreExtensions
    {
        public static void DetachAllEntities(this DbContext context)
        {
            foreach (var entity in context.ChangeTracker.Entries())
            {
                if (entity.State != EntityState.Detached)
                {
                    entity.State = EntityState.Detached;
                }
            }
        }
    }
}
