// ReSharper disable UnusedMember.Global

using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    // ReSharper disable once UnusedMember.Global
    public class EmailHistoryDto
    {
        public Guid? NotificationId { get; set; }
        public int? OpportunityId { get; set; }
        public int? OpportunityItemId { get; set; }
        public int EmailTemplateId { get; set; }
        public string EmailTemplateName { get; set; }
        public string Status { get; set; }
        public string SentTo { get; set; }
        public string CreatedBy { get; set; }
    }
}