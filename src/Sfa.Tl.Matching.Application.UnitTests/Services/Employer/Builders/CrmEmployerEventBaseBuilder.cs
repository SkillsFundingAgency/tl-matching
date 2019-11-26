using System;
using Sfa.Tl.Matching.Models.Event;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders
{
    internal class CrmEmployerEventBaseBuilder
    {
        private readonly CrmEmployerEventBase _crmEmployerEventBase;

        public CrmEmployerEventBaseBuilder()
        {
            _crmEmployerEventBase = new CrmEmployerEventBase
            {
                AccountId = Guid.NewGuid().ToString(),
                Name = "Test",

                AddressLine = "Test",
                PostCode = "Test",

                CustomerTypeCode = new Customertypecode { Value = 200005 },

                PrimaryContactId = new PrimaryContactId { Name = "Test" },
                ContactEmail = "Test@test.com",
                ContactTelephone1 = "0123456789",

                Phone = "NOT_IN_USE",
                EmailAddress = "NOT_IN_USE",

                OwnerEmail = "NOT_IN_USE",
                OwnerIdName = "Test",

                Alias = "Test",

                CeasedTrading = 0,
                EmployerManagement = null,
            };
        }

        internal CrmEmployerEventBaseBuilder WithZeroAupaStatus()
        {
            _crmEmployerEventBase.sfa_aupa = new SfaAupa { Value = 0 };

            return this;
        }

        internal CrmEmployerEventBaseBuilder WithValidAupaStatus()
        {
            _crmEmployerEventBase.sfa_aupa = new SfaAupa { Value = 229660000 };

            return this;
        }
        public CrmEmployerEventBase Build() => _crmEmployerEventBase;
    }
}