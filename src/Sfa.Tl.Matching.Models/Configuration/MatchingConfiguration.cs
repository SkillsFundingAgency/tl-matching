﻿
using System;

namespace Sfa.Tl.Matching.Models.Configuration
{
    public class MatchingConfiguration
    {
        public AuthenticationConfig Authentication { get; set; }
        public string AuthorisedAdminUserEmail { get; set; }
        public string BlobStorageConnectionString { get; set; }
        public string GovNotifyApiKey { get; set; }
        public Guid EmailDeliveryStatusToken { get; set; }
        public bool SendEmailEnabled { get; set; }
        public string PostcodeRetrieverBaseUrl { get; set; }
        public string SqlConnectionString { get; set; }
        public string GoogleMapsApiBaseUrl { get; set; }
        public string GoogleMapsApiKey { get; set; }
        public string BankHolidaysJsonUrl { get; set; }
        public bool EmployerFeedbackEmailsEnabled { get; set; }
        public int EmployerFeedbackWorkingDayInMonth { get; set; }
        public bool ProviderFeedbackEmailsEnabled { get; set; }
        public int ProviderFeedbackWorkingDayInMonth { get; set; }
        public string MatchingServiceSupportEmailAddress { get; set; }
    }
}