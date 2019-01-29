using System.ComponentModel.DataAnnotations;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SelectedImportDataViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "A file type must be selected")]
        public int Id { get; set; }

        public string Name { get; }

        public DataImportType Type { get; }
        public string ContentType { get; }
        public byte[] Data { get; }

        public string FileName => $"{Type.ToString()}/{Name}";
    }
}