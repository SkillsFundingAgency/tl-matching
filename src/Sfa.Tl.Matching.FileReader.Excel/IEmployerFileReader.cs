using System.IO;

namespace Sfa.Tl.Matching.FileReader.Excel
{
    public interface IEmployerFileReader // TODO AU MOVE
    {
        EmployerLoadResult Load(Stream stream);
    }
}