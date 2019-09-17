using System;
using Sfa.Tl.Matching.Models.Event;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders
{
    public static class CrmEmployerEventBaseBuilder
    {
        public static CrmEmployerEventBase Buiild(bool isValid) => new CrmEmployerEventBase
        {
            ContactEmail = "Test@test.com",
            ContactTelephone1 = "0123456789",
            Name = "Test",
            PrimaryContactId = null,
            accountid = Guid.NewGuid().ToString(),
            address1_line1 = "Test",
            address1_postalcode = "Test",
            customertypecode = new Customertypecode { Value = 200005 },
            emailaddress1 = "Test",
            owneremail = "Test",
            owneridname = "test",
            sfa_alias = "Test",
            sfa_aupa = new SfaAupa { Value = isValid ? 229660000 : 0 },
            sfa_ceasedtrading = 0,
            sfa_employermanagement = null,
            telephone1 = "Test"
        };
    }
}