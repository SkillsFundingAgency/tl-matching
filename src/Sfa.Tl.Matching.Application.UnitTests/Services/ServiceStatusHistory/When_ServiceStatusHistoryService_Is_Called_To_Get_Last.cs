using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ServiceStatusHistory.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
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

            var mockDbSet = new ServiceStatusHistoryBuilder()
                .Build()
                .AsQueryable()
                .BuildMockDbSet();

            var serviceStatusHistoryRepository = Substitute.For<IRepository<Domain.Models.ServiceStatusHistory>>();
            serviceStatusHistoryRepository.GetManyAsync(Arg.Any<Expression<Func<Domain.Models.ServiceStatusHistory, bool>>>()).Returns(mockDbSet);

            var serviceStatusHistoryService = new ServiceStatusHistoryService(mapper, serviceStatusHistoryRepository);
            _result = serviceStatusHistoryService.GetLatestServiceStatusHistoryAsync()
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ViewModel_Is_Correct()
        {
            _result.IsOnline.Should().BeFalse();
        }
    }
}