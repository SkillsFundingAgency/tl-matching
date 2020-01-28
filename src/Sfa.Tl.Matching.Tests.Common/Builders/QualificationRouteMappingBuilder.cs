using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common.Extensions;

namespace Sfa.Tl.Matching.Tests.Common.Builders
{
    public class QualificationRouteMappingBuilder
    {
        private readonly MatchingDbContext _context;
        private readonly QualificationBuilder _qualificationBuilder;

        public IList<QualificationRouteMapping> QualificationRouteMappings { get; }
        
        public QualificationRouteMappingBuilder(MatchingDbContext context)
        {
            _context = context;
            _qualificationBuilder = new QualificationBuilder(_context);
            QualificationRouteMappings = new List<QualificationRouteMapping>();
        }

        public QualificationRouteMappingBuilder CreateQualificationRouteMappings(int numberOfQualificationRouteMappings = 1, string createdBy = null,
            DateTime? createdOn = null, string modifiedBy = null, DateTime? modifiedOn = null)
        {
            //var qualificationRouteMapping = new QualificationRouteMapping
            //{
            //    Id = 1,
            //    RouteId = 2,
            //    Source = "Test",
            //    CreatedBy = createdBy,
            //    CreatedOn = createdOn ?? default(DateTime),
            //    ModifiedBy = modifiedBy,
            //    ModifiedOn = modifiedOn,
            //    Qualification = _qualificationBuilder.CreateQualification(
            //        1, "1234567X", "Full Qualification Title",
            //        "Short Title"
            //    ).Qualifications.Last()
            //};
            //QualificationRouteMappings.Add(qualificationRouteMapping);

            //qualificationRouteMapping = new QualificationRouteMapping
            //{
            //    Id = 2,
            //    RouteId = 3,
            //    Source = "Test",
            //    CreatedBy = createdBy,
            //    CreatedOn = createdOn ?? default(DateTime),
            //    ModifiedBy = modifiedBy,
            //    ModifiedOn = modifiedOn,
            //    Qualification = _qualificationBuilder.CreateQualification(
            //        1, 
            //        "7654321X",
            //        "Another Qualification Title",
            //        "Another Short Title"
            //        ).Qualifications.Last()
            //};
            //QualificationRouteMappings.Add(qualificationRouteMapping);
            
            for (var i = 0; i < numberOfQualificationRouteMappings; i++)
            {
                var qualificationRouteMappingNumber = i + 1;
                var qualificationNumber = i + 1;
                var routeId = i + 2;

                var qualification =  _qualificationBuilder.CreateQualification(
                    qualificationRouteMappingNumber,
                    $"{qualificationNumber}000X",
                            $"Qualification Title {qualificationRouteMappingNumber}",
                            $"Short Title {qualificationRouteMappingNumber}",
                    createdBy, createdOn, modifiedBy, modifiedOn
                    ).Qualifications.Last();

                CreateQualificationRouteMapping(qualificationRouteMappingNumber, routeId,
                    "Test",
                    qualification,
                    createdBy, createdOn, modifiedBy, modifiedOn);
            }

            return this;
        }

        public QualificationRouteMappingBuilder CreateQualificationRouteMapping(int id, int routeId, 
            string source = "Test", 
            Qualification qualification = null,
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
                Qualification = qualification
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
