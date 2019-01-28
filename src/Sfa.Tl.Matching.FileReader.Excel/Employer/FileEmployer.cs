
using System;

namespace Sfa.Tl.Matching.FileReader.Excel.Employer
{
    public class FileEmployer
    {
        public int Id { get; set; }
        public Guid CrmId { get; set; }
        public string CompanyName { get; set; }
        public string AlsoKnownAs { get; set; }
        public string PrimaryContact { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PostCode { get; set; }
        public string Owner { get; set; }
    }
}