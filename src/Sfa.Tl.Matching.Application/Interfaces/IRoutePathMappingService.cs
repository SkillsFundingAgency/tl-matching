using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IRoutePathMappingService
    {
 
        Task<int> ImportQualificationPathMapping(QualificationRoutePathMappingFileImportDto fileImportDto);

        void IndexQualificationPathMapping();
    }
}
