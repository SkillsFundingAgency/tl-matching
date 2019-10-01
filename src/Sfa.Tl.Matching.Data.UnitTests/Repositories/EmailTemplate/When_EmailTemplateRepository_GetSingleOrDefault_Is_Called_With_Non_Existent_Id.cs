using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailTemplate.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailTemplate
{
    public class When_EmailTemplateRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.EmailTemplate _result;

        public When_EmailTemplateRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailTemplate>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidEmailTemplateBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.EmailTemplate>(logger, dbContext);
                _result = repository.GetSingleOrDefaultAsync(x => x.Id == 2)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_EmailTemplate_Is_Returned() =>
            _result.Should().BeNull();
    }
}