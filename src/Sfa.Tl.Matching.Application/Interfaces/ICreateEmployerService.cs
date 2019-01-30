using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface ICreateEmployerService
    {
        Task<int> Process(Stream stream);
    }
}