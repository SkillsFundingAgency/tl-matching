using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailHistory.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailHistory
{
    public class When_EmailHistoryRepository_GetMany_Is_Called
    {
        private readonly IEnumerable<Domain.Models.EmailHistory> _result;

        public When_EmailHistoryRepository_GetMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailHistory>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidEmailHistoryListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.EmailHistory>(logger, dbContext);
                _result = repository.GetMany().ToList();
            }
        }

        [Fact]
        public void Then_The_Expected_Number_Of_EmailHistories_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_EmailHistory_Id_Is_Returned() =>
            _result.First().Id.Should().Be(1);
        
        [Fact]
        public void Then_EmailHistory_ReferralId_Is_Returned() =>
            _result.First().ReferralId.Should().Be(1);

        [Fact]
        public void Then_EmailHistory_EmailTemplateId_Is_Returned() =>
            _result.First().EmailTemplateId.Should().Be(2);
        
        [Fact]
        public void Then_EmailHistory_SentTo_Is_Returned() =>
            _result.First().SentTo.Should().BeEquivalentTo("recipient@test.com");

        [Fact]
        public void Then_EmailHistory_CopiedTo_Is_Returned()
            => _result.First().CopiedTo.Should().BeEquivalentTo("copy@test.com");

        [Fact]
        public void Then_EmailHistory_BlindCopiedTo_Is_Returned()
            => _result.First().BlindCopiedTo.Should().BeEquivalentTo("blindcopy@test.com");

        [Fact]
        public void Then_EmailHistory_CreatedBy_Is_Returned() =>
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_EmailHistory_CreatedOn_Is_Returned() =>
            _result.First().CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_EmailHistory_ModifiedBy_Is_Returned() =>
            _result.First().ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
        
        [Fact]
        public void Then_EmailHistory_ModifiedOn_Is_Returned() =>
            _result.First().ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}