using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmployerFileImportDto : FileImportDto
    {
        public override Stream FileDataStream { get; set; }
        public override int? NumberOfHeaderRows => 1;

        [Column(Order = 0)] public string CrmId { get; set; }
        [Column(Order = 1)] public string CompanyName { get; set; }
        [Column(Order = 2)] public string AlsoKnownAs { get; set; }
        [Column(Order = 3)] public string Aupa { get; set; }
        [Column(Order = 4)] public string CompanyType { get; set; }
        [Column(Order = 4)] public string PrimaryContact { get; set; }
        [Column(Order = 6)] public string Phone { get; set; }
        [Column(Order = 7)] public string Email { get; set; }
        [Column(Order = 8)] public string PostCode { get; set; }
        [Column(Order = 9)] public string Owner { get; set; }
    }
}