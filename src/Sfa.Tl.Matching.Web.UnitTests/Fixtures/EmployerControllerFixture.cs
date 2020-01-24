using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.UnitTests.Fixtures
{
    public class EmployerControllerFixture<TDto, TViewModel>
    where TDto: class
    where TViewModel: class
    {
        internal int OpportunityId;
        internal int OpportunityItemId;
        internal string CompanyName;
        internal string EmployerContact;
        internal string EmployerContactPhone;
        internal string EmployerContactEmail;
        internal Guid EmployerCrmId;
        internal string ModifiedBy;

        internal EmployerController Sut;
        internal readonly IEmployerService EmployerService;
        internal readonly IOpportunityService OpportunityService;
        internal readonly IHttpContextAccessor HttpcontextAccesor;
        internal readonly IReferralService ReferralService;

        public EmployerControllerFixture()
        {
            EmployerService = Substitute.For<IEmployerService>();
            OpportunityService = Substitute.For<IOpportunityService>();
            HttpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            ReferralService = Substitute.For<IReferralService>();

            var config = FixtureExtension.ConfigureAutoMapper<TDto, TViewModel>(HttpcontextAccesor);
            var mapper = new Mapper(config);

            SetValues();

            Sut = new EmployerController(EmployerService, OpportunityService, ReferralService, mapper);
        }

        private void SetValues()
        {
            OpportunityId = 12;
            OpportunityItemId = 34;
            CompanyName = "CompanyName";
            EmployerContact = "EmployerContact";
            EmployerContactPhone = "123456789";
            EmployerContactEmail = "EmployerContactEmail";
            ModifiedBy = "ModifiedBy";
            EmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111");
        }
    }
}
