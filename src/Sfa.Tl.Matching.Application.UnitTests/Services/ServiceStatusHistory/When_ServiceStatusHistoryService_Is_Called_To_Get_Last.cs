﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ServiceStatusHistory
{
    public class When_ServiceStatusHistoryService_Is_Called_To_Get_Last
    {
        private readonly ServiceStatusHistoryViewModel _result;

        public When_ServiceStatusHistoryService_Is_Called_To_Get_Last()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(ServiceStatusHistoryMapper).Assembly));
            var mapper = new Mapper(config);

            var serviceStatusHistories = new List<Domain.Models.ServiceStatusHistory>
            {
                new Domain.Models.ServiceStatusHistory
                {
                    Id = 1,
                    IsOnline = false
                }
            }.AsQueryable();

            var mockSet = Substitute.For<DbSet<Domain.Models.ServiceStatusHistory>, IAsyncEnumerable<Domain.Models.ServiceStatusHistory>, IQueryable<Domain.Models.ServiceStatusHistory>>();

            // ReSharper disable once SuspiciousTypeConversion.Global
            ((IAsyncEnumerable<Domain.Models.ServiceStatusHistory>)mockSet).GetEnumerator()
                .Returns(new FakeAsyncEnumerator<Domain.Models.ServiceStatusHistory>(serviceStatusHistories.GetEnumerator()));
            ((IQueryable<Domain.Models.ServiceStatusHistory>)mockSet).Provider.Returns(
                new FakeAsyncQueryProvider<Domain.Models.ServiceStatusHistory>(serviceStatusHistories.Provider));
            ((IQueryable<Domain.Models.ServiceStatusHistory>)mockSet).Expression.Returns(serviceStatusHistories.Expression);
            ((IQueryable<Domain.Models.ServiceStatusHistory>)mockSet).ElementType.Returns(serviceStatusHistories.ElementType);
            ((IQueryable<Domain.Models.ServiceStatusHistory>)mockSet).GetEnumerator().Returns(serviceStatusHistories.GetEnumerator());

            var contextOptions = new DbContextOptions<MatchingDbContext>();
            var mockContext = Substitute.For<MatchingDbContext>(contextOptions);
            mockContext.Set<Domain.Models.ServiceStatusHistory>().Returns(mockSet);

            var serviceStatusHistoryRepository =
                new GenericRepository<Domain.Models.ServiceStatusHistory>(NullLogger<GenericRepository<Domain.Models.ServiceStatusHistory>>.Instance, mockContext);

            var serviceStatusHistoryService = new ServiceStatusHistoryService(mapper, serviceStatusHistoryRepository);
            _result = serviceStatusHistoryService.GetLatestServiceStatusHistory()
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ViewModel_Is_Correct()
        {
            _result.IsOnline.Should().BeFalse();
        }
    }
}