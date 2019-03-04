using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification
{
    public class When_QualificationRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.Qualification _result;

        public When_QualificationRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Qualification>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidQualificationBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.Qualification>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 2)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_Qualification_Is_Returned() =>
            _result.Should().BeNull();
    }
}