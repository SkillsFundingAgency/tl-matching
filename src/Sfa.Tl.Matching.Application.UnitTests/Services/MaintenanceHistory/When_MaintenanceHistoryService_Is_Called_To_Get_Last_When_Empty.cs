using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.MaintenanceHistory
{
    public class When_MaintenanceHistoryService_Is_Called_To_Get_Last_When_Empty
    {
        private readonly MaintenanceViewModel _result;
        private readonly IRepository<Domain.Models.MaintenanceHistory> _maintenanceHistoryRepository;

        public When_MaintenanceHistoryService_Is_Called_To_Get_Last_When_Empty()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(MaintenanceMapper).Assembly));
            var mapper = new Mapper(config);

            _maintenanceHistoryRepository = Substitute.For<IRepository<Domain.Models.MaintenanceHistory>>();
            _maintenanceHistoryRepository.GetLastOrDefault(
                Arg.Any<Expression<Func<Domain.Models.MaintenanceHistory, bool>>>())
                .ReturnsNull();

            var maintenanceHistoryService = new MaintenanceHistoryService(mapper, _maintenanceHistoryRepository);
            _result = maintenanceHistoryService.GetLatestMaintenanceHistory()
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetLastOrDefault_Is_Called_Exactly_Once()
        {
            _maintenanceHistoryRepository
                .Received(1)
                .GetLastOrDefault(Arg.Any<Expression<Func<Domain.Models.MaintenanceHistory, bool>>>());
        }

        [Fact]
        public void Then_ViewModel_Is_Correct()
        {
            _result.IsOnline.Should().BeTrue();
        }
    }
}