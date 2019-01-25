using System.Collections.Generic;

namespace Sfa.Tl.Matching.FileReader.Excel.Employer
{
    public class EmployerLoadResult // TODO AU MOVE
    {
        public List<FileEmployer> Data { get; set; }
        public string Error { get; set; }
    }
}