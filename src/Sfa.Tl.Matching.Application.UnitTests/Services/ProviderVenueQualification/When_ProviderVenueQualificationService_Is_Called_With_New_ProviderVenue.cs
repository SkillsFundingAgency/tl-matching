using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenueQualification.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenueQualification
{
    public class When_ProviderVenueQualificationService_Is_Called_With_New_ProviderVenue
    {
        private readonly IProviderVenueService _providerVenueService;
        private readonly IProviderQualificationService _providerQualificationService;
        private readonly IQualificationRouteMappingService _qualificationRouteMappingService;
        private readonly IQualificationService _qualificationService;
        private readonly IRoutePathService _routePathService;

        private readonly IEnumerable<ProviderVenueQualificationUpdateResultsDto> _results;

        public When_ProviderVenueQualificationService_Is_Called_With_New_ProviderVenue()
        {
            var providerService = Substitute.For<IProviderService>();
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerQualificationService = Substitute.For<IProviderQualificationService>();
            _qualificationRouteMappingService = Substitute.For<IQualificationRouteMappingService>();
            _qualificationService = Substitute.For<IQualificationService>();
            _routePathService = Substitute.For<IRoutePathService>();

            providerService.SearchAsync(10000001)
                .Returns((ProviderSearchResultDto)null);

            providerService.CreateProviderAsync(Arg.Any<CreateProviderDetailViewModel>()).Returns(1);
            _providerVenueService.CreateVenueAsync(Arg.Any<AddProviderVenueViewModel>()).Returns(1);

            _providerVenueService
                .GetVenueWithTrimmedPostcodeAsync(1, "CV1 2WT")
                .Returns((ProviderVenueDetailViewModel)null);

            _providerVenueService
                .GetVenueAsync(1)
                .Returns(new ProviderVenueDetailViewModel
                {
                    Id = 1,
                    ProviderId = 1,
                    Postcode = "CV1 2WT",
                    IsEnabledForReferral = true,
                    Source = "Import",
                    Qualifications = new List<QualificationDetailViewModel>()
                });

            _qualificationService.GetQualificationAsync(Arg.Any<string>()).Returns((QualificationDetailViewModel)null);
            _providerQualificationService.GetProviderQualificationAsync(Arg.Any<int>(), Arg.Any<int>()).Returns((ProviderQualificationDto)null);
            _routePathService.GetRouteSummaryByNameAsync(Arg.Any<string>()).Returns((RouteSummaryViewModel)null);

            var providerVenueQualificationService = new ProviderVenueQualificationService
            (
                providerService,
                _providerVenueService,
                _providerQualificationService,
                _qualificationService,
                _routePathService,
                _qualificationRouteMappingService
            );

            var dtoList = new ValidProviderVenueQualificationDtoListBuilder()
                .AddVenue()
                .Build();

            _results = providerVenueQualificationService.Update(dtoList).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Results_Has_One_Item()
        {
            _results.Should().NotBeNull();
            var resultsList = _results.ToList();
            resultsList.Count.Should().Be(1);
        }

        [Fact]
        public void Then_Results_Has_Expected_Values()
        {
            _results.First().UkPrn.Should().Be(10000001);
            _results.First().VenuePostcode.Should().Be("CV1 2WT");
            _results.First().LarId.Should().BeNull();
        }

        [Fact]
        public void Then_Results_Has_No_Errors()
        {
            _results.First().HasErrors.Should().BeFalse();
        }

        [Fact]
        public void Then_ProviderVenueService_GetVenueWithTrimmedPostcodeAsync_Is_Called_Exactly_Once()
        {
            _providerVenueService
                .Received(1)
                .GetVenueWithTrimmedPostcodeAsync(1, "CV1 2WT");
        }

        [Fact]
        public void Then_ProviderVenueService_GetVenueAsync_Is_Called_Exactly_Once()
        {
            _providerVenueService
                .Received(1)
                .GetVenueAsync(1);
        }

        [Fact]
        public void Then_ProviderVenueService_UpdateVenueAsync_Is_Called_Exactly_Once_With_Expected_Values()
        {
            _providerVenueService
                .Received(1)
                .UpdateVenueAsync(Arg.Is<ProviderVenueDetailViewModel>(
                    p =>
                        p.Id == 1 &&
                        p.ProviderId == 1 &&
                        p.Postcode == "CV1 2WT" &&
                        p.Name == "Test Provider Venue" &&
                        p.Source == "Import"));
        }

        [Fact]
        public void Then_ProviderVenueService_UpdateVenueAsync_Is_Not_Called_To_Remove_Venue()
        {
            _providerVenueService
                .DidNotReceive()
                .UpdateVenueAsync(Arg.Any<RemoveProviderVenueViewModel>());
        }

        [Fact]
        public void Then_ProviderVenueService_UpdateVenueToNotRemovedAsync_Is_Not_Called()
        {
            _providerVenueService
                .DidNotReceive()
                .UpdateVenueToNotRemovedAsync(Arg.Any<RemoveProviderVenueViewModel>());
        }

        [Fact]
        public void Then_ProviderVenueService_GetRemoveProviderVenueViewModelAsync_Is_Not_Called()
        {
            _providerVenueService
                .DidNotReceive()
                .GetRemoveProviderVenueViewModelAsync(Arg.Any<int>());
        }

        [Fact]
        public void Then_ProviderQualificationService_GetProviderQualificationAsync_Is_Not_Called()
        {
            _providerQualificationService
                .DidNotReceive()
                .GetProviderQualificationAsync(Arg.Any<int>(), Arg.Any<int>());
        }

        [Fact]
        public void Then_QualificationService_GetQualificationAsync_Is_Not_Called()
        {
            _qualificationService.
                DidNotReceive()
                .GetQualificationAsync(Arg.Any<string>());
        }

        [Fact]
        public void Then_RoutePathService_GetRouteSummaryByNameAsync_Is_Not_Called()
        {
            _routePathService
                .DidNotReceive()
                .GetRouteSummaryByNameAsync(Arg.Any<string>());
        }

        [Fact]
        public void Then_QualificationRouteMappingService_GetQualificationRouteMappingAsync_Is_Not_Called()
        {
            _qualificationRouteMappingService
                .DidNotReceive()
                .GetQualificationRouteMappingAsync(Arg.Any<int>(), Arg.Any<int>());
        }

        [Fact]
        public void Then_ProviderVenueService_CreateVenueAsync_Is_Called_Exactly_Once_With_Expected_Values()
        {
            _providerVenueService
                .Received(1)
                .CreateVenueAsync(Arg.Is<AddProviderVenueViewModel>(
                    p =>
                        p.ProviderId == 1 &&
                        p.Postcode == "CV1 2WT" &&
                        p.Source == "Import"));
        }
    }
}
