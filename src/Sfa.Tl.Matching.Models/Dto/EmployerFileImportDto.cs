using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text.RegularExpressions;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmployerFileImportDto : FileImportDto
    {
        public override Stream FileDataStream { get; set; }
        public override int? NumberOfHeaderRows => 1;

        [Column(Order = 0)] public string CrmId { get; set; }
        [Column(Order = 3)] public string CompanyName { get; set; }
        [Column(Order = 4)] public string AlsoKnownAs { get; set; }
        [Column(Order = 5)] public string Aupa { get; set; }
        [Column(Order = 6)] public string CompanyType { get; set; }
        [Column(Order = 7)] public string PrimaryContact { get; set; }
        [Column(Order = 8)] public string Phone { get; set; }
        [Column(Order = 9)] public string Email { get; set; }
        [Column(Order = 10)] public string PostCode { get; set; }
        [Column(Order = 11)] public string Owner { get; set; }
    }
}