using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Qualification.Builders
{
    public class ValidQualificationBuilder
    {
        public Domain.Models.Qualification Build() => new Domain.Models.Qualification
        {
            Id = 1,
            LarsId = "10042982",
            Title = "Title",
            ShortTitle = "Short Title",
            QualificationSearch = "TitleShortTitle",
            ShortTitleSearch = "ShortTitle",
            CreatedBy = "CreatedBy",
            ModifiedBy = "ModifiedBy"
        };

        public Domain.Models.Qualification BuildWithRoutes() => new Domain.Models.Qualification
        {
            Id = 1,
            LarsId = "10042982",
            Title = "Title",
            ShortTitle = "Short Title",
            QualificationSearch = "TitleShortTitle",
            ShortTitleSearch = "ShortTitle",
            CreatedBy = "CreatedBy",
            ModifiedBy = "ModifiedBy",
            QualificationRouteMapping =
                new List<QualificationRouteMapping>
                { 
                    new QualificationRouteMapping
                    {
                        Id = 1,
                        RouteId = 1
                    },
                    new QualificationRouteMapping
                    {
                        Id = 2,
                        RouteId = 2
                    }
                }
        };
    }
}
