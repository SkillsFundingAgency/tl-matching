using Sfa.Tl.Matching.Application.FileReader.LocalEnterprisePartnershipStaging;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.LocalEnterprisePartnership.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LocalEnterprisePartnership.Validation
{
    public class LocalEnterprisePartnershipStagingFileImportFixture
    {
        public LocalEnterprisePartnershipStagingDataValidator Validator;
        public LocalEnterprisePartnershipStagingFileImportDto Dto;

        public LocalEnterprisePartnershipStagingFileImportFixture()
        {
            Dto = new ValidLocalEnterprisePartnershipStagingFileImportDtoBuilder().Build();
            Validator = new LocalEnterprisePartnershipStagingDataValidator();
        }
    }
}