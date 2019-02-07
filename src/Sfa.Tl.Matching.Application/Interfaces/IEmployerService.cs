using System.IO;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmployerService
    {
        void ImportEmployer(Stream dataStream);
        void GetEmployerByName();
        void CreateEmployer();
        void UpdateEmployer();
    }
}