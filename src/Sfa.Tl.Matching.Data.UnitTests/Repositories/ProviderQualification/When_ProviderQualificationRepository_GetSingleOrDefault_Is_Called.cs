using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderQualification.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderQualification
{
    public class When_ProviderQualificationRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.ProviderQualification _result;

        public When_ProviderQualificationRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.ProviderQualification>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidProviderQualificationListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.ProviderQualification>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_ProviderQualification_Id_Is_Returned() =>
            _result.Id.Should().Be(1);
        
        [Fact]
        public void Then_ProviderQualification_ProviderVenueId_Is_Returned() =>
            _result.ProviderVenueId.Should().Be(1);

        [Fact]
        public void Then_ProviderQualification_QualificationId_Is_Returned() =>
            _result.QualificationId.Should().Be(1);

        [Fact]
        public void Then_ProviderQualification_NumberOfPlacements_Is_Returned()
            => _result.NumberOfPlacements.Should().Be(1);

        [Fact]
        public void Then_ProviderQualification_Source_Is_Returned() =>
            _result.Source.Should().BeEquivalentTo("Test");
        
        [Fact]
        public void Then_ProviderQualification_CreatedBy_Is_Returned() =>
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_ProviderQualification_CreatedOn_Is_Returned() =>
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_ProviderQualification_ModifiedBy_Is_Returned() =>
            _result.ModifiedBy.Should().Be(EntityCreationConstants.ModifiedByUser);
        
        [Fact]
        public void Then_ProviderQualification_ModifiedOn_Is_Returned() =>
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}