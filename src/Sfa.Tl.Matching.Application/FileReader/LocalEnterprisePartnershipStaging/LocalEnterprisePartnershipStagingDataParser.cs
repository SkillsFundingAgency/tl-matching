using System.Collections.Generic;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.LocalEnterprisePartnershipStaging
{
    public class LocalEnterprisePartnershipStagingDataParser : IDataParser<LocalEnterprisePartnershipStagingDto>
    {
        public IEnumerable<LocalEnterprisePartnershipStagingDto> Parse(FileImportDto fileImportDto)
        {
            if (!(fileImportDto is LocalEnterprisePartnershipStagingFileImportDto data)) return null;

            var localEnterprisePartnershipStagingDto = new LocalEnterprisePartnershipStagingDto
            {
                Code = data.Code,
                Name = data.Name,
                CreatedBy = data.CreatedBy
            };

            return new List<LocalEnterprisePartnershipStagingDto> { localEnterprisePartnershipStagingDto };
        }
    }
}