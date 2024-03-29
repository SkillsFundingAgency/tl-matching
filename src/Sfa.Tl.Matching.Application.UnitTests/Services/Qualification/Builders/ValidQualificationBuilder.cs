﻿namespace Sfa.Tl.Matching.Application.UnitTests.Services.Qualification.Builders
{
    public class ValidQualificationBuilder
    {
        public Domain.Models.Qualification Build() => new()
        {
            Id = 1,
            LarId = "10042982",
            Title = "Title",
            ShortTitle = "Short Title",
            QualificationSearch = "TitleShortTitle",
            ShortTitleSearch = "ShortTitle",
            CreatedBy = "CreatedBy",
            ModifiedBy = "ModifiedBy"
        };
    }
}
