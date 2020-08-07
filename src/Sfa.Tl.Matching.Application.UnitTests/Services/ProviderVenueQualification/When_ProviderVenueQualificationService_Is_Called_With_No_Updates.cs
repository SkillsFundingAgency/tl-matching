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
    public class When_ProviderVenueQualificationService_Is_Called_With_No_Updates
    {
        private readonly IProviderService _providerService;
        private readonly IProviderVenueService _providerVenueService;
        private readonly IProviderQualificationService _providerQualificationService;
        private readonly IQualificationRouteMappingService _qualificationRouteMappingService;
        private readonly IQualificationService _qualificationService;
        private readonly IRoutePathService _routePathService;

        private readonly IEnumerable<ProviderVenueQualificationUpdateResultsDto> _results;

        public When_ProviderVenueQualificationService_Is_Called_With_No_Updates()
        {
            _providerService = Substitute.For<IProviderService>();
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerQualificationService = Substitute.For<IProviderQualificationService>();
            _qualificationRouteMappingService = Substitute.For<IQualificationRouteMappingService>();
            _qualificationService = Substitute.For<IQualificationService>();
            _routePathService = Substitute.For<IRoutePathService>();

            var dtoList = new ValidProviderVenueQualificationDtoListBuilder()
                .AddVenue()
                .AddQualificationWithRoutes()
                .Build();
            var dto = dtoList.First();

            _providerService.SearchAsync(10000001)
                .Returns(new ProviderSearchResultDto
                {
                    Id = 1,
                    UkPrn = dto.UkPrn,
                    Name = dto.ProviderName
                });

            _providerService.GetProviderDetailByIdAsync(1)
                .Returns(new ProviderDetailViewModel
                {
                    UkPrn = dto.UkPrn,
                    Name = dto.ProviderName,
                    DisplayName = dto.DisplayName,
                    IsCdfProvider = dto.IsCdfProvider,
                    IsEnabledForReferral = dto.IsEnabledForReferral,
                    PrimaryContact = dto.PrimaryContact,
                    PrimaryContactEmail = dto.PrimaryContactEmail,
                    PrimaryContactPhone = dto.PrimaryContactPhone,
                    SecondaryContact = dto.SecondaryContact,
                    SecondaryContactEmail = dto.SecondaryContactEmail,
                    SecondaryContactPhone = dto.SecondaryContactPhone
                });

            _providerVenueService
                .GetVenueAsync(1, "CV1 2WT")
                .Returns(new ProviderVenueDetailViewModel
                {
                    Id = 1,
                    ProviderId = 1,
                    Postcode = dto.VenuePostcode,
                    Name = dto.VenueName,
                    IsEnabledForReferral = dto.VenueIsEnabledForReferral,
                    Qualifications = new List<QualificationDetailViewModel>()
                });

            _qualificationService
                .GetQualificationAsync("1234567X")
                .Returns(new QualificationDetailViewModel
                {
                    Id = 10,
                    LarId = "1234567X",
                    Title = "Full qualification title",
                    ShortTitle = "Short qualification title"
                });
            
            _providerQualificationService
                .GetProviderQualificationAsync(Arg.Any<int>(), Arg.Any<int>())
                .Returns(new ProviderQualificationDto
                {
                    ProviderVenueId = 1,
                    QualificationId = 10
                });

            _routePathService
                .GetRouteSummaryByNameAsync("Agriculture, environmental and animal care")
                .Returns(new RouteSummaryViewModel
                {
                    Id = 1,
                    Name = "Agriculture, environmental and animal care"
                });
            _routePathService
                .GetRouteSummaryByNameAsync("Digital")
                .Returns(new RouteSummaryViewModel
                {
                    Id = 3,
                    Name = "Digital"
                });

            _qualificationRouteMappingService
                .GetQualificationRouteMappingAsync(1, 10)
                .Returns(new QualificationRouteMappingViewModel
                {
                    QualificationId= 10,
                    RouteId = 1
                });
            _qualificationRouteMappingService
                .GetQualificationRouteMappingAsync(3, 10)
                .Returns(new QualificationRouteMappingViewModel
                {
                    QualificationId = 10,
                    RouteId = 3
                });

            var providerVenueQualificationService = new ProviderVenueQualificationService
                (
                   _providerService,
                   _providerVenueService,
                   _providerQualificationService,
                   _qualificationService,
                   _routePathService,
                   _qualificationRouteMappingService
                );

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
        public void Then_ProviderService_SearchAsync_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .SearchAsync(Arg.Any<long>());
        }

        [Fact]
        public void Then_ProviderService_GetProviderDetailByIdAsync_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .GetProviderDetailByIdAsync(Arg.Any<int>());
        }

        [Fact]
        public void Then_ProviderService_CreateProviderAsync_Is_Not_Called()
        {
            _providerService
                .DidNotReceive()
                .CreateProviderAsync(Arg.Any<CreateProviderDetailViewModel>());
        }

        [Fact]
        public void Then_ProviderService_UpdateProviderDetailAsync_Is_Not_Called()
        {
            _providerService
                .DidNotReceive()
                .UpdateProviderDetailAsync(Arg.Any<ProviderDetailViewModel>());
        }

        [Fact]
        public void Then_ProviderVenueService_CreateVenueAsync_Is_Not_Called()
        {
            _providerVenueService
                .DidNotReceive()
                .CreateVenueAsync(Arg.Any<AddProviderVenueViewModel>());
        }

        [Fact]
        public void Then_ProviderVenueService_UpdateVenueAsync_Is_Not_Called()
        {
            _providerVenueService
                .DidNotReceive()
                .UpdateVenueAsync(Arg.Any<ProviderVenueDetailViewModel>());
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
        public void Then_ProviderQualificationService_GetProviderQualificationAsync_Is_Called_Exactly_Once()
        {
            _providerQualificationService
                .Received(1)
                .GetProviderQualificationAsync(Arg.Any<int>(), Arg.Any<int>());
        }

        [Fact]
        public void Then_ProviderQualificationService_GetProviderQualificationAsync_Is_Called_With_Expected_Values()
        {
            _providerQualificationService
                .Received(1)
                .GetProviderQualificationAsync(1, 10);
        }

        [Fact]
        public void Then_QualificationService_GetQualificationAsync_Is_Called_With_Expected_Values()
        {
            _qualificationService.
                Received(1)
                .GetQualificationAsync("1234567X");
        }

        [Fact]
        public void Then_RoutePathService_GetRouteSummaryByNameAsync_Is_Called_Exactly_Twice()
        {
            _routePathService
                .Received(2)
                .GetRouteSummaryByNameAsync(Arg.Any<string>());
        }

        [Fact]
        public void Then_RoutePathService_GetRouteSummaryByNameAsync_Is_Called_Exactly_Once_With_Agriculture_Route()
        {
            _routePathService
                .Received(1)
                .GetRouteSummaryByNameAsync("Agriculture, environmental and animal care");
        }

        [Fact]
        public void Then_RoutePathService_GetRouteSummaryByNameAsync_Is__Exactly_Once_With_Digital_Route()
        {
            _routePathService
                .Received(1)
                .GetRouteSummaryByNameAsync("Digital");
        }

        [Fact]
        public void Then_QualificationRouteMappingService_GetQualificationRouteMappingAsync_Is_Called_Exactly_Twice()
        {
            _qualificationRouteMappingService
                .Received(2)
                .GetQualificationRouteMappingAsync(Arg.Any<int>(), Arg.Any<int>());
        }

        [Fact]
        public void Then_QualificationRouteMappingService_GetQualificationRouteMappingAsync_Is_Called_Exactly_Once_With_Agriculture_Route()
        {
            _qualificationRouteMappingService
                .Received(1)
                .GetQualificationRouteMappingAsync(1, 10);
        }

        [Fact]
        public void Then_QualificationRouteMappingService_GetQualificationRouteMappingAsync_Is_Called_Exactly_Once_With_Digital_Route()
        {
            _qualificationRouteMappingService
                .Received(1)
                .GetQualificationRouteMappingAsync(3, 10);
        }

        [Fact]
        public void Then_QualificationService_CreateQualificationEntityAsync_Is_Not_Called()
        {
            _qualificationService
                .DidNotReceive()
                .CreateQualificationEntityAsync(Arg.Any<MissingQualificationViewModel>());
        }

        [Fact]
        public void Then_ProviderQualificationService_CreateProviderQualificationAsync_Is_Not_Called()
        {
            _providerQualificationService
                .DidNotReceive()
                .CreateProviderQualificationAsync(Arg.Any<AddQualificationViewModel>());
        }

        [Fact]
        public void Then_ProviderQualificationService_RemoveProviderQualificationAsync_Is_Not_Called()
        {
            _providerQualificationService
                .DidNotReceive()
                .RemoveProviderQualificationAsync(Arg.Any<int>(), Arg.Any<int>());
        }

        [Fact]
        public void Then_QualificationRouteMappingService_CreateQualificationRouteMappingAsync_Is_Not_Called()
        {
            _qualificationRouteMappingService
                .DidNotReceive()
                .CreateQualificationRouteMappingAsync(Arg.Any<QualificationRouteMappingViewModel>());
        }
    }
}
