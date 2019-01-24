using System.IO;

namespace Sfa.Tl.Matching.FileReader.Excel.Employer
{
    public interface IEmployerFileReader // TODO AU MOVE
    {
        EmployerLoadResult Load(Stream stream);
    }
}