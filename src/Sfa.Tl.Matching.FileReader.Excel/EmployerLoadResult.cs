using System.Collections.Generic;

namespace Sfa.Tl.Matching.FileReader.Excel
{
    public class EmployerLoadResult // TODO AU MOVE
    {
        public List<Employer.Employer> Data { get; set; }
        public string Error { get; set; }
    }
}