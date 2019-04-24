using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ConfirmSendProviderEmailViewModel
    {
        public int ProviderCount { get; set; }
        [Required(ErrorMessage = "You must tell us whether you want to send these emails")]
        public bool? SendEmail { get; set; }
    }
}