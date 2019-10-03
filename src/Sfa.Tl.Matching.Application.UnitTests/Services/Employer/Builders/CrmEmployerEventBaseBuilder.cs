using System;
using Sfa.Tl.Matching.Models.Event;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders
{
    public static class CrmEmployerEventBaseBuilder
    {
        public static CrmEmployerEventBase Build(bool isValid) => new CrmEmployerEventBase
        {
            accountid = Guid.NewGuid().ToString(),
            Name = "Test",

            address1_line1 = "Test",
            address1_postalcode = "Test",

            customertypecode = new Customertypecode { Value = 200005 },

            PrimaryContactId = new PrimaryContactId { name = "Test" },
            ContactEmail = "Test@test.com",
            ContactTelephone1 = "0123456789",
            
            telephone1 = "NOT_IN_USE",
            emailaddress1 = "NOT_IN_USE",

            owneremail = "NOT_IN_USE",
            owneridname = "Test",

            sfa_alias = "Test",
            sfa_aupa = new SfaAupa { Value = isValid ? 229660000 : 0 },

            sfa_ceasedtrading = 0,
            sfa_employermanagement = null,
        };
    }
}