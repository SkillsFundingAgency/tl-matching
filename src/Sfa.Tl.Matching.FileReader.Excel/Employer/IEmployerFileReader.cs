using System.IO;

namespace Sfa.Tl.Matching.FileReader.Excel.Employer
{
    public interface IEmployerFileReader
    {
        EmployerLoadResult Load(Stream stream);
    }
}