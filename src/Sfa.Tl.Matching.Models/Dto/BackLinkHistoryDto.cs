using System;
using System.Text;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class BackLinkHistoryDto
    {
        public string CurrentUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
