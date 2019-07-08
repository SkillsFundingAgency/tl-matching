using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.AutoDomain
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute() : base(GetDefaultFixture)
        {

        }

        public static IFixture GetDefaultFixture()
        {
            var autoMoqCustomization = new AutoMoqCustomization
            {
                ConfigureMembers = true
            };

            return new Fixture()
                .Customize(new InMemoryDbContext())
                .Customize(new OpportunityCustomization())
                .Customize(autoMoqCustomization);
        }
    }

    public class InMemoryDbContext : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            //var options = new DbContextOptionsBuilder<MatchingDbContext>()
            //    .UseInMemoryDatabase(Guid.NewGuid().ToString())
            //    .EnableSensitiveDataLogging()
            //    .Options;

            //var dbcontext = new MatchingDbContext(options);
            //fixture.Register(() => dbcontext);

            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            //fixture.Register(() =>
            //    new GenericRepository<Referral>(fixture.Create<ILogger<GenericRepository<Referral>>>(), dbcontext));

            //fixture.Register(() =>
            //    new GenericRepository<OpportunityItem>(fixture.Create<ILogger<GenericRepository<OpportunityItem>>>(), dbcontext));

            //fixture.Register(() =>
            //    new GenericRepository<ProvisionGap>(fixture.Create<ILogger<GenericRepository<ProvisionGap>>>(), dbcontext));

            //fixture.Register(() =>
            //    new GenericRepository<Referral>(fixture.Create<ILogger<GenericRepository<Referral>>>(), dbcontext));

            //fixture.Register(() => new OpportunityRepository(fixture.Create<ILogger<OpportunityRepository>>(), dbcontext));

        }
    }

    public class OpportunityCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new PropertyNameOmitter("ProviderVenue", "Location"));
            //fixture.Customizations.Add(new SetOpportunityId());
        }
    }

    internal class PropertyNameOmitter : ISpecimenBuilder
    {
        private readonly IEnumerable<string> _names;

        internal PropertyNameOmitter(params string[] names)
        {
            this._names = names;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var propInfo = request as PropertyInfo;
            if (propInfo != null && _names.Contains(propInfo.Name))
                return new OmitSpecimen();

            return new NoSpecimen();
        }
    }
}
