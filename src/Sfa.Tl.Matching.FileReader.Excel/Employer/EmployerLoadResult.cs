using System.Collections.Generic;

namespace Sfa.Tl.Matching.FileReader.Excel.Employer
{
    public class EmployerLoadResult
    {
        public List<FileEmployer> Data { get; }
        public string Error { get; }

        public EmployerLoadResult(List<FileEmployer> data, string error)
        {
            Data = data;
            Error = error;
        }
    }
}