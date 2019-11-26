using System;
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
    public class When_EmailTemplateRepository_Update_Is_Called
    {
        private readonly Domain.Models.EmailTemplate _result;

        public When_EmailTemplateRepository_Update_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailTemplate>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var entity = new ValidEmailTemplateBuilder().Build();
                dbContext.Add(entity);
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.EmailTemplate>(logger, dbContext);

                entity.TemplateId = new Guid("98706811-DCE7-4938-87D4-CD14CB6F86A4").ToString();
                entity.TemplateName = "UpdatedTemplate";

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
            _result.TemplateId.Should().BeEquivalentTo("98706811-DCE7-4938-87D4-CD14CB6F86A4");
            _result.TemplateName.Should().Be("UpdatedTemplate");

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
