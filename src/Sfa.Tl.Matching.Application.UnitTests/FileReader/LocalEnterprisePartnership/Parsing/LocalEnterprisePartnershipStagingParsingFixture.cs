using Sfa.Tl.Matching.Application.FileReader.LocalEnterprisePartnershipStaging;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.LocalEnterprisePartnership.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LocalEnterprisePartnership.Parsing
{
    public class LocalEnterprisePartnershipStagingParsingFixture
    {
        public LocalEnterprisePartnershipStagingDataParser Parser;
        public LocalEnterprisePartnershipStagingFileImportDto Dto;

        public LocalEnterprisePartnershipStagingParsingFixture()
        {
            Dto = new ValidLocalEnterprisePartnershipStagingFileImportDtoBuilder().Build();
            Parser = new LocalEnterprisePartnershipStagingDataParser();
        }
    }
}