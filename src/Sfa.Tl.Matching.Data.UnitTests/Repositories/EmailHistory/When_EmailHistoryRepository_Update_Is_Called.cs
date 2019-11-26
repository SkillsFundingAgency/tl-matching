using System;
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
    public class When_EmailHistoryRepository_Update_Is_Called
    {
        private readonly Domain.Models.EmailHistory _result;

        public When_EmailHistoryRepository_Update_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailHistory>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var entity = new ValidEmailHistoryBuilder().Build();
                dbContext.Add(entity);
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.EmailHistory>(logger, dbContext);

                entity.SentTo = "updated.recipient@test.com";
                entity.CopiedTo = "updated.copy@test.com";
                entity.BlindCopiedTo = "updated.blindcopy@test.com";

                entity.ModifiedOn = new DateTime(2019, 11, 01, 12, 30, 00);
                entity.ModifiedBy = "UpdateTestUser";

                repository.UpdateAsync(entity).GetAwaiter().GetResult();

                _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.OpportunityId.Should().Be(1);
            _result.EmailTemplateId.Should().Be(2);
            _result.SentTo.Should().BeEquivalentTo("updated.recipient@test.com");
            _result.CopiedTo.Should().BeEquivalentTo("updated.copy@test.com");
            _result.BlindCopiedTo.Should().BeEquivalentTo("updated.blindcopy@test.com");

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
