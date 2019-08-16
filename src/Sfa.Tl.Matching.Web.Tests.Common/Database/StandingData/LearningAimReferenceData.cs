using System;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Web.Tests.Common.Database.StandingData
{
    internal class LearningAimReferenceData
    {
        internal static LearningAimReference[] Create()
        {
            var learningAimReferences = new[]
            {
                new LearningAimReference
                {
                    Id = 1,
                    LarId = "12345678",
                    Title = "Qualification Title",
                    AwardOrgLarId = "a",
                    SourceCreatedOn = new DateTime(2019, 1, 1),
                    SourceModifiedOn = new DateTime(2019, 1, 1),
                    CreatedOn = new DateTime(2019, 1, 1),
                    CreatedBy = "Dev Surname"
                }
            };

            return learningAimReferences;
        }
    }
}