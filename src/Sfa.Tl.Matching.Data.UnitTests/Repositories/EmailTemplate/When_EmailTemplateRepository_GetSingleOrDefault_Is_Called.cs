using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailTemplate.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailTemplate
{
    public class When_EmailTemplateRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.EmailTemplate _result;

        public When_EmailTemplateRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailTemplate>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidEmailTemplateListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.EmailTemplate>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.TemplateId.Should().BeEquivalentTo("1777EF96-70F5-4537-A6B1-41E8A0D8E0EC");
            _result.TemplateName.Should().BeEquivalentTo("TestTemplate");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}