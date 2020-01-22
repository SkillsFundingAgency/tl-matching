using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common.Extensions;

namespace Sfa.Tl.Matching.Tests.Common.Builders
{
    public class RouteBuilder
    {
        private readonly MatchingDbContext _context;

        public IList<Route> Routes { get; }

        public RouteBuilder(MatchingDbContext context)
        {
            _context = context;
            Routes = new List<Route>();
        }

        public RouteBuilder CreateRoutes(int numberOfRoutes = 1, string createdBy = null,
            DateTime? createdOn = null, string modifiedBy = null, DateTime? modifiedOn = null)
        {
            for (var i = 0; i < numberOfRoutes; i++)
            {
                var routeNumber = i + 1;
                CreateRoute(routeNumber, $"Route {routeNumber}",
                    $"Keyword{routeNumber}", $"Route {routeNumber} summary",
                    createdBy, createdOn, modifiedBy, modifiedOn);
            }

            return this;
        }

        public RouteBuilder CreateRoute(int id, string name, string keywords = "", string summary = "",
            string createdBy = null, DateTime? createdOn = null,
            string modifiedBy = null, DateTime? modifiedOn = null)
        {
            var route = new Route
            {
                Id = id,
                Name = name,
                Keywords = keywords,
                Summary = summary,
                CreatedBy = createdBy,
                CreatedOn = createdOn ?? default(DateTime),
                ModifiedBy = modifiedBy,
                ModifiedOn = modifiedOn,
            };

            Routes.Add(route);

            return this;
        }

        public RouteBuilder ClearData()
        {
            if (!Routes.IsNullOrEmpty())
                _context.Route.RemoveRange(Routes);

            _context.SaveChanges();

            Routes.Clear();

            return this;
        }

        public RouteBuilder SaveData()
        {
            if (!Routes.IsNullOrEmpty())
                _context.AddRange(Routes);

            _context.SaveChanges();

            return this;
        }
    }
}