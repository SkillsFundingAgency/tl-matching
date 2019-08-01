using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailHistory.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailHistory
{
    public class When_EmailHistoryRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.EmailHistory _result;

        public When_EmailHistoryRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailHistory>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidEmailHistoryListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.EmailHistory>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_EmailHistory_Id_Is_Returned() =>
            _result.Id.Should().Be(1);
        
        [Fact]
        public void Then_EmailHistory_OpportunityId_Is_Returned() =>
            _result.OpportunityId.Should().Be(1);

        [Fact]
        public void Then_EmailHistory_EmailTemplateId_Is_Returned() =>
            _result.EmailTemplateId.Should().Be(2);
        
        [Fact]
        public void Then_EmailHistory_SentTo_Is_Returned() =>
            _result.SentTo.Should().BeEquivalentTo("recipient@test.com");

        [Fact]
        public void Then_EmailHistory_CopiedTo_Is_Returned()
            => _result.CopiedTo.Should().BeEquivalentTo("copy@test.com");

        [Fact]
        public void Then_EmailHistory_BlindCopiedTo_Is_Returned()
            => _result.BlindCopiedTo.Should().BeEquivalentTo("blindcopy@test.com");

        [Fact]
        public void Then_EmailHistory_CreatedBy_Is_Returned() =>
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_EmailHistory_CreatedOn_Is_Returned() =>
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_EmailHistory_ModifiedBy_Is_Returned() =>
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
        
        [Fact]
        public void Then_EmailHistory_ModifiedOn_Is_Returned() =>
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}