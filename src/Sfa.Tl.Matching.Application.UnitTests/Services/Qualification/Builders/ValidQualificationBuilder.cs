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
            CreatedBy = "CreatedBy",
            ModifiedBy = "ModifiedBy"
        };
    }
}
