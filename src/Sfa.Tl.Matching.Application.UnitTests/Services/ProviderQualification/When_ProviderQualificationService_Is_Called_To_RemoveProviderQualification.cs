using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQualification
{
    public class When_ProviderQualificationService_Is_Called_To_RemoveProviderQualification
    {
        private readonly MatchingDbContext _mockContext;

        public When_ProviderQualificationService_Is_Called_To_RemoveProviderQualification()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(QualificationMapper).Assembly));
            var mapper = new Mapper(config);

            var mockSet = Substitute.For<DbSet<Domain.Models.ProviderQualification>, IAsyncEnumerable<Domain.Models.ProviderQualification>, IQueryable<Domain.Models.ProviderQualification>>();

            var providerQualifications = new List<Domain.Models.ProviderQualification> {
                new Domain.Models.ProviderQualification
                    {
                        Id = 1,
                        ProviderVenueId = 1,
                        QualificationId = 2
                    }}.AsQueryable();

            // ReSharper disable once SuspiciousTypeConversion.Global
            ((IAsyncEnumerable<Domain.Models.ProviderQualification>)mockSet).GetAsyncEnumerator()
                .Returns(new FakeAsyncEnumerator<Domain.Models.ProviderQualification>(providerQualifications.GetEnumerator()));
            ((IQueryable<Domain.Models.ProviderQualification>)mockSet).Provider.Returns(
                new FakeAsyncQueryProvider<Domain.Models.ProviderQualification>(providerQualifications.Provider));
            ((IQueryable<Domain.Models.ProviderQualification>)mockSet).Expression.Returns(providerQualifications.Expression);
            ((IQueryable<Domain.Models.ProviderQualification>)mockSet).ElementType.Returns(providerQualifications.ElementType);
            ((IQueryable<Domain.Models.ProviderQualification>)mockSet).GetEnumerator().Returns(providerQualifications.GetEnumerator());

            var contextOptions = new DbContextOptions<MatchingDbContext>();
            _mockContext = Substitute.For<MatchingDbContext>(contextOptions, false);
            _mockContext.Set<Domain.Models.ProviderQualification>().Returns(mockSet);

            IRepository<Domain.Models.ProviderQualification> repository = new GenericRepository<Domain.Models.ProviderQualification>(NullLogger<GenericRepository<Domain.Models.ProviderQualification>>.Instance, _mockContext);

            var providerQualificationService = new ProviderQualificationService(mapper, repository);

            providerQualificationService.RemoveProviderQualificationAsync(1, 2).GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Then_ProviderQualificationRepository_Delete_Is_Called_Exactly_Once_With_Expected_Values()
        {
            _mockContext
                .Received(1)
                .RemoveRange(Arg.Is<IList<Domain.Models.ProviderQualification>>(
                    p => p.First().ProviderVenueId == 1 &&
                            p.First().QualificationId == 2
                            ));

           await _mockContext.Received(1).SaveChangesAsync();
        }
    }
}