using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmployerService
    {
        Task ImportEmployer(Stream dataStream);
        void GetEmployerByName();
        void CreateEmployer();
        void UpdateEmployer();
    }
}