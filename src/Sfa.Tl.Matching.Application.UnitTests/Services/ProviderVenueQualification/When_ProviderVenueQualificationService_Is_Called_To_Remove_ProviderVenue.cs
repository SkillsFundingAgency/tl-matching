﻿using System.Collections.Generic;
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
    public class When_ProviderVenueQualificationService_Is_Called_To_Remove_ProviderVenue
    {
        private readonly IProviderVenueService _providerVenueService;
        private readonly IProviderQualificationService _providerQualificationService;
        private readonly IQualificationRouteMappingService _qualificationRouteMappingService;
        private readonly IQualificationService _qualificationService;
        private readonly IRoutePathService _routePathService;

        private readonly IEnumerable<ProviderVenueQualificationUpdateResultsDto> _results;

        public When_ProviderVenueQualificationService_Is_Called_To_Remove_ProviderVenue()
        {
            var providerService = Substitute.For<IProviderService>();
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerQualificationService = Substitute.For<IProviderQualificationService>();
            _qualificationRouteMappingService = Substitute.For<IQualificationRouteMappingService>();
            _qualificationService = Substitute.For<IQualificationService>();
            _routePathService = Substitute.For<IRoutePathService>();

            var dtoList = new ValidProviderVenueQualificationDtoListBuilder()
                .AddVenue(isRemoved: true)
                .Build();
            var dto = dtoList.First();

            providerService.SearchAsync(10000001)
                .Returns(new ProviderSearchResultDto
                {
                    Id = 1,
                    UkPrn = dto.UkPrn,
                    Name = dto.ProviderName
                });

            providerService.GetProviderDetailByIdAsync(1)
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
                .GetVenueWithTrimmedPostcodeAsync(1, "CV1 2WT")
                .Returns(new ProviderVenueDetailViewModel
                {
                    Id = 1,
                    ProviderId = 1,
                    Postcode = dto.VenuePostcode,
                    Name = dto.VenueName,
                    IsEnabledForReferral = dto.VenueIsEnabledForReferral,
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

            _results = providerVenueQualificationService.UpdateAsync(dtoList).GetAwaiter().GetResult();
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
        public void Then_ProviderVenueService_GetVenueAsync_Is_Not_Called()
        {
            _providerVenueService
                .DidNotReceive()
                .GetVenueAsync(1);
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
        public void Then_ProviderVenueService_CreateVenueAsync_Is_Not_Called()
        {
            _providerVenueService
                .Received(1)
                .DidNotReceive()
                .CreateVenueAsync(Arg.Any<AddProviderVenueViewModel>());
        }

        [Fact]
        public void Then_ProviderVenueService_UpdateVenueAsync_Is_Called_To_Remove_Venue_With_Expected_Values()
        {
            _providerVenueService
                .Received(1)
                .UpdateVenueAsync(Arg.Is<ProviderVenueDetailViewModel>(
                    p =>
                        p.Id == 1 &&
                        p.ProviderId == 1 &&
                        p.Postcode == "CV1 2WT" &&
                        p.IsRemoved));
        }

        [Fact]
        public void Then_ProviderVenueService_UpdateVenueAsync_Is_Not_Called_With_RemoveProviderVenueViewModel()
        {
            _providerVenueService
                .DidNotReceive()
                .UpdateVenueAsync(Arg.Any<RemoveProviderVenueViewModel>());
        }
    }
}
