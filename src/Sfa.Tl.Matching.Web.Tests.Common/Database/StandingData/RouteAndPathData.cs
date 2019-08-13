using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Web.Tests.Common.Database.StandingData
{
    internal class RouteAndPathData
    {
        internal static Route[] Create()
        {
            var routes = new[]
            {
                new Route
                {
                    Id = 1,
                    Name = "Agriculture, environmental and animal care",
                    Summary = "Includes: animal care and management; agriculture, land management and production",
                    CreatedOn = new DateTime(2019, 1, 1),
                    CreatedBy = "Dev Surname",
                    Path = new List<Path>
                    {
                        new Path
                        {
                            Id = 1,
                            RouteId = 1,
                            Name = "Agriculture, land management and production",
                            CreatedOn = new DateTime(2019, 1, 1),
                            CreatedBy = "Dev Surname",
                        },
                        new Path
                        {
                            Id = 2,
                            RouteId = 1,
                            Name = "Animal care and management",
                            CreatedOn = new DateTime(2019, 1, 1),
                            CreatedBy = "Dev Surname",
                        }
                    }
                },
                new Route
                {
                    Id = 2,
                    Name = "Business and administration",
                    Summary = "Includes: management and administration; human resources (HR)",
                    CreatedOn = new DateTime(2019, 1, 1),
                    CreatedBy = "Dev Surname",
                    Path = new List<Path>
                    {
                        new Path
                        {
                            Id = 3,
                            RouteId = 2,
                            Name = "Human resources",
                            CreatedOn = new DateTime(2019, 1, 1),
                            CreatedBy = "Dev Surname",
                        },
                        new Path
                        {
                            Id = 4,
                            RouteId = 2,
                            Name = "Management and administration",
                            CreatedOn = new DateTime(2019, 1, 1),
                            CreatedBy = "Dev Surname",
                        }
                    }
                }
            };

            return routes;
        }
    }
}