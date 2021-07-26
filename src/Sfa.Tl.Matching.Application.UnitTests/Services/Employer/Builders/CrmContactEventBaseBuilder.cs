using System;
using Sfa.Tl.Matching.Models.Event;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders
{
    public static class CrmContactEventBaseBuilder
    {
        public static CrmContactEventBase Build() => new CrmContactEventBase
        {
            ContactId = Guid.NewGuid().ToString(),

            CustomerTypeCode = new Customertypecode { Value = 200005 },
            ParentCustomerId = new ParentCustomerId { Id = Guid.NewGuid().ToString() },
            
            Fullname = "Test",
            EmailAddress = "Test@test.com",
            Telephone = "0123456789",

            MobilePhone = "NOT_IN_USE",

            AddressLine1 = "Test",
            AddressLine2 = "Test",
            AddressLine3 = "Test",
            City = "Test", 
            County = "Test",
            PostCode = "Test",
        };
    }
}