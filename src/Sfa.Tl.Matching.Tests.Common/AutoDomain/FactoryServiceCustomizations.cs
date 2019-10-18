using AutoFixture;
using AutoFixture.Kernel;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class FactoryServiceCustomizations : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register<IRepository<BankHoliday>>(fixture.Create<GenericRepository<BankHoliday>>);
            fixture.Register<IDateTimeProvider>(fixture.Create<DateTimeProvider>);

            fixture.Behaviors.Add(new NullRecursionBehavior());
            fixture.Customizations.Add(new TypeRelay(typeof(FeedbackService), typeof(EmployerFeedbackService)));
            fixture.Customizations.Add(new TypeRelay(typeof(FeedbackService), typeof(ProviderFeedbackService)));
        }
    }
}
