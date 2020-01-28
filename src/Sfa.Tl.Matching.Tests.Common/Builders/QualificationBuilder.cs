using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common.Extensions;

namespace Sfa.Tl.Matching.Tests.Common.Builders
{
    public class QualificationBuilder
    {
        private readonly MatchingDbContext _context;

        public IList<Qualification> Qualifications { get; }

        public QualificationBuilder(MatchingDbContext context)
        {
            _context = context;
            Qualifications = new List<Qualification>();
        }

        public QualificationBuilder CreateQualifications(int numberOfQualifications = 1, string createdBy = null,
            DateTime? createdOn = null, string modifiedBy = null, DateTime? modifiedOn = null)
        {
            for (var i = 0; i < numberOfQualifications; i++)
            {
                var qualificationNumber = i + 1;
                CreateQualification(qualificationNumber,
                    $"{qualificationNumber}000X",
                    $"Title {qualificationNumber}",
                    $"Short Title {qualificationNumber}", 
                    createdBy, createdOn, modifiedBy, modifiedOn);
            }

            return this;
        }

        public QualificationBuilder CreateQualification(int id, string larId, 
            string title = "", string shortTitle = "",
            string createdBy = null, DateTime? createdOn = null,
            string modifiedBy = null, DateTime? modifiedOn = null)
        {
            var qualification = new Qualification
            {
                Id = id,
                LarId = larId,
                Title = title,
                ShortTitle = shortTitle,
                CreatedBy = createdBy,
                CreatedOn = createdOn ?? default(DateTime),
                ModifiedBy = modifiedBy,
                ModifiedOn = modifiedOn,
            };

            Qualifications.Add(qualification);

            return this;
        }

        public QualificationBuilder ClearData()
        {
            if (!Qualifications.IsNullOrEmpty())
                _context.Qualification.RemoveRange(Qualifications);

            _context.SaveChanges();

            Qualifications.Clear();

            return this;
        }

        public QualificationBuilder SaveData()
        {
            if (!Qualifications.IsNullOrEmpty())
                _context.AddRange(Qualifications);

            _context.SaveChanges();

            return this;
        }
    }
}
