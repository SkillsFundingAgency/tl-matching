using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification
{
    public class When_QualificationRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.Qualification _result;

        public When_QualificationRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Qualification>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidQualificationListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.Qualification>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Qualification_Id_Is_Returned() =>
            _result.Id.Should().Be(1);
        
        [Fact]
        public void Then_Qualification_LarsId_Is_Returned() =>
            _result.LarsId.Should().BeEquivalentTo("1000");

        [Fact]
        public void Then_Qualification_Title_Is_Returned() =>
            _result.Title.Should().BeEquivalentTo("Title");

        [Fact]
        public void Then_Qualification_ShortTitle_Is_Returned()
            => _result.ShortTitle.Should().BeEquivalentTo("ShortTitle");
       
        [Fact]
        public void Then_Qualification_CreatedBy_Is_Returned() =>
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_Qualification_CreatedOn_Is_Returned() =>
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_Qualification_ModifiedBy_Is_Returned() =>
            _result.ModifiedBy.Should().Be(EntityCreationConstants.ModifiedByUser);
        
        [Fact]
        public void Then_Qualification_ModifiedOn_Is_Returned() =>
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}