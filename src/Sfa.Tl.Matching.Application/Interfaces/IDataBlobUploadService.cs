using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IDataBlobUploadService
    {
        Task UploadAsync(DataUploadDto dto);

        Task UploadFromStreamAsync(DataStreamUploadDto dto);
    }
}