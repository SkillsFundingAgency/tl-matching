using System.Collections.Generic;

namespace Sfa.Tl.Matching.FileReader.Excel.Employer
{
    public class EmployerLoadResult // TODO AU MOVE
    {
        public List<Employer> Data { get; set; }
        public string Error { get; set; }
    }
}