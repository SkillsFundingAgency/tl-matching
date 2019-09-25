using System;
using Sfa.Tl.Matching.Models.Event;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders
{
    public static class CrmContactEventBaseBuilder
    {
        public static CrmContactEventBase Buiild(bool isValid) => new CrmContactEventBase
        {
            contactid = Guid.NewGuid().ToString(),

            customertypecode = new Customertypecode { Value = 200005 },
            parentcustomerid = new Parentcustomerid { id = Guid.NewGuid().ToString() },
            
            fullname = "Test",
            emailaddress1 = "Test@test.com",
            telephone1 = "0123456789",

            mobilephone = "NOT_IN_USE",

            address1_line1 = "Test",
            address1_line2 = "Test",
            address1_line3 = "Test",
            address1_city = "Test", 
            address1_county = "Test",
            address1_postalcode = "Test",
        };
    }
}