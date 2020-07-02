using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Employer.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Employer
{
    public class When_EmployerRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.Employer _result;

        public When_EmployerRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Employer>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.Add(new ValidEmployerBuilder().Build());
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.Employer>(logger, dbContext);
            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 2)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_Employer_Is_Returned() =>
            _result.Should().BeNull();
    }
}