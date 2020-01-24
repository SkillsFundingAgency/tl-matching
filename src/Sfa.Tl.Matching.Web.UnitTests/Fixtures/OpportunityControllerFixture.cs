using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.UnitTests.Fixtures
{
    public class OpportunityControllerFixture<TDto, TViewModel> : OpportunityControllerFixture
        where TDto : class
        where TViewModel : class
    {


        public OpportunityControllerFixture()
        {
            OpportunityService = Substitute.For<IOpportunityService>();
            HttpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var mapper = new Mapper(ConfigureMapper);

            SetValues();

            Sut = new OpportunityController(OpportunityService, mapper);
        }

        public sealed override MapperConfiguration ConfigureMapper => FixtureExtension.ConfigureAutoMapper<TDto, TViewModel>(HttpcontextAccesor);
    }

    public class OpportunityControllerFixture
    {
        internal int OpportunityId;
        internal int OpportunityItemId;
        internal string CompanyName;
        internal string CompanyNameAka;
        internal string Postcode;
        internal string JobRole;
        internal int BasketItemCount;
        internal int ReferralIdToDelete;

        internal IOpportunityService OpportunityService;
        internal IHttpContextAccessor HttpcontextAccesor;

        internal OpportunityController Sut;

        public OpportunityControllerFixture()
        {
            OpportunityService = Substitute.For<IOpportunityService>();
            HttpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var mapper = new Mapper(ConfigureMapper);

            SetValues();

            Sut = new OpportunityController(OpportunityService, mapper);
        }

        public virtual MapperConfiguration ConfigureMapper => FixtureExtension.ConfigureAutoMapper();

        public void SetValues()
        {
            OpportunityId = 1;
            OpportunityItemId = 2;
            CompanyName = "Company Name1";
            CompanyNameAka = "Also Known As 1";
            Postcode = "PostCode1";
            JobRole = "JobRole1";
            BasketItemCount = 2;
            ReferralIdToDelete = 3;
        }

    }
}
