using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IFileWriter<in TDto> where TDto : class, new()
    {
        Task<Stream> WriteReport(TDto data);
    }
}