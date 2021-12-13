using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LocalEnterprisePartnership.Builders
{
    public class ValidLocalEnterprisePartnershipStagingFileImportDtoBuilder
    {
        public LocalEnterprisePartnershipStagingFileImportDto Build() => new()
        {
            Code = "E37000007",
            Name = "Cumbria",
            CreatedBy = "CreatedBy"
        };
    }
}