using System.IO;
using Sfa.Tl.Matching.Core.Enums;
using Sfa.Tl.Matching.FileReader.Excel.Employer;

namespace Sfa.Tl.Matching.Application.Employer
{
    public class EmployerDataLoader
    {
        public EmployerLoadResult Load(Stream stream, FileType fileType)
        {
            var reader = EmployerReaderFactory.Create(fileType);
            var loadResult = reader.Load(stream);

            return loadResult;
        }
    }
}