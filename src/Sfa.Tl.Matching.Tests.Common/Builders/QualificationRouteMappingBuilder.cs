using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common.Extensions;

namespace Sfa.Tl.Matching.Tests.Common.Builders
{
    public class QualificationRouteMappingBuilder
    {
        private readonly MatchingDbContext _context;

        public IList<QualificationRouteMapping> QualificationRouteMappings { get; }

        public QualificationRouteMappingBuilder(MatchingDbContext context)
        {
            _context = context;
            QualificationRouteMappings = new List<QualificationRouteMapping>();
        }

        public QualificationRouteMappingBuilder CreateQualificationRouteMappings(int numberOfQualificationRouteMappings = 1, string createdBy = null,
            DateTime? createdOn = null, string modifiedBy = null, DateTime? modifiedOn = null)
        {
            //TODO: Refactor to call method below
            var qualificationRouteMapping = new Domain.Models.QualificationRouteMapping
            {
                Id = 1,
                RouteId = 2,
                Source = "Test",
                CreatedBy = createdBy,
                CreatedOn = createdOn ?? default(DateTime),
                ModifiedBy = modifiedBy,
                ModifiedOn = modifiedOn,
                Qualification = new Domain.Models.Qualification
                {
                    LarId = "1234567X",
                    Title = "Full Qualification Title",
                    ShortTitle = "Short Title"
                }
            };
            QualificationRouteMappings.Add(qualificationRouteMapping);

            qualificationRouteMapping = new Domain.Models.QualificationRouteMapping
            {
                Id = 2,
                RouteId = 3,
                Source = "Test",
                CreatedBy = createdBy,
                CreatedOn = createdOn ?? default(DateTime),
                ModifiedBy = modifiedBy,
                ModifiedOn = modifiedOn,
                Qualification = new Domain.Models.Qualification
                {
                    LarId = "7654321X",
                    Title = "Another Qualification Title",
                    ShortTitle = "Another Short Title"
                }
            };
            QualificationRouteMappings.Add(qualificationRouteMapping);
            return this;


            for (var i = 0; i < numberOfQualificationRouteMappings; i++)
            {
                var qualificationRouteMappingNumber = i + 1;
                var routeId = i + 2;
                CreateQualificationRouteMapping(qualificationRouteMappingNumber, routeId,
                    "Test",
                    createdBy, createdOn, modifiedBy, modifiedOn);
            }

            return this;
        }

        public QualificationRouteMappingBuilder CreateQualificationRouteMapping(int id, int routeId, 
            string source = "Test",
            string createdBy = null, DateTime? createdOn = null,
            string modifiedBy = null, DateTime? modifiedOn = null)
        {
            var qualificationRouteMapping = new QualificationRouteMapping
            {
                Id = id,
                RouteId = routeId,
                Source = source,
                CreatedBy = createdBy,
                CreatedOn = createdOn ?? default(DateTime),
                ModifiedBy = modifiedBy,
                ModifiedOn = modifiedOn,
            };

            QualificationRouteMappings.Add(qualificationRouteMapping);

            return this;
        }

        public QualificationRouteMappingBuilder ClearData()
        {
            if (!QualificationRouteMappings.IsNullOrEmpty())
                _context.QualificationRouteMapping.RemoveRange(QualificationRouteMappings);

            _context.SaveChanges();

            QualificationRouteMappings.Clear();

            return this;
        }

        public QualificationRouteMappingBuilder SaveData()
        {
            if (!QualificationRouteMappings.IsNullOrEmpty())
                _context.AddRange(QualificationRouteMappings);

            _context.SaveChanges();

            return this;
        }
    }
}
