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

                sfa_ceasedtrading = 0,
                sfa_employermanagement = null,
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