//using FluentAssertions;
//using Microsoft.Extensions.Logging;
//using NSubstitute;
//using Sfa.Tl.Matching.Application.UnitTests.Data.Employer.Builders;
//using Sfa.Tl.Matching.Data.Repositories;
//using Xunit;

//namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Employer
//{
//    public class When_EmployerRepository_CreateMany_Is_Called
//    {
//        private readonly int _result;

//        public When_EmployerRepository_CreateMany_Is_Called()
//        {
//            var logger = Substitute.For<ILogger<EmployerRepository>>();

//            using (var dbContext = InMemoryDbContext.Create())
//            {
//                var data = new ValidEmployerListBuilder().Build();

//                var repository = new EmployerRepository(logger, dbContext);
//                _result = repository.CreateMany(data)
//                    .GetAwaiter().GetResult();
//            }
//        }

//        [Fact]
//        public void Then_Two_Records_Should_Have_Been_Created() =>
//            _result.Should().Be(2);
//    }
//}