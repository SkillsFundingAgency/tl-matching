﻿using System.ComponentModel;

namespace Sfa.Tl.Matching.Models.Enums
{
    public enum EmailTemplateName
    {
        EmployerFeedback,
        EmployerFeedbackV2,
        [Description("Employer referral confirmation")]
        EmployerReferral,
        [Description("Employer referral confirmation")]
        EmployerReferralComplex,
        [Description("Employer referral confirmation")]
        EmployerReferralV3,
        [Description("Employer referral confirmation")]
        EmployerReferralV4,
        [Description("Employer referral confirmation")]
        EmployerReferralV5,
        [Description("Provider quarterly")]
        ProviderQuarterlyUpdate,
        [Description("Provider referral")]
        ProviderReferral,
        [Description("Provider referral")]
        ProviderReferralComplex,
        [Description("Provider referral")]
        ProviderReferralV3,
        [Description("Provider referral")]
        ProviderReferralV4,
        [Description("Provider referral")]
        ProviderReferralV5,
        ProviderFeedback,
        ProviderFeedbackV2,
        EmployerAupaBlank,
        EmailDeliveryStatus
    }
}