using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Tl.Matching.Models.NotificationCallback
{
    public class CallbackPayLoad
    {
        public Guid id { get; set; }
        public string reference { get; set; }
        public string to { get; set; }
        public string status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? completed_at { get; set; }
        public DateTime? sent_at { get; set; }
        public string notification_type { get; set; }

    }
}
