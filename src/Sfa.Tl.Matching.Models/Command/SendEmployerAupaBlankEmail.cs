using System;

namespace Sfa.Tl.Matching.Models.Command
{
    public class SendEmployerAupaBlankEmail
    {
        public Guid CrmId { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
    }
}