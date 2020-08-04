using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
    public class When_ProviderVenueQualificationService_Is_Called_To_Unremove_ProviderVenue
    {
        private readonly IProviderService _providerService;
        private readonly IProviderVenueService _providerVenueService;
        private readonly IProviderQualificationService _providerQualificationService;
        private readonly IQualificationService _qualificationService;
        private readonly IRoutePathService _routePathService;
        private readonly IQualificationRouteMappingService _qualificationRouteMappingService;

        private readonly IEnumerable<ProviderVenueQualificationUpdateResultsDto> _results;

        public When_ProviderVenueQualificationService_Is_Called_To_Unremove_ProviderVenue()
        {
            _providerService = Substitute.For<IProviderService>();
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerQualificationService = Substitute.For<IProviderQualificationService>();
            _qualificationService = Substitute.For<IQualificationService>();
            _routePathService = Substitute.For<IRoutePathService>();
            _qualificationRouteMappingService = Substitute.For<IQualificationRouteMappingService>();

            var dtoList = new ValidProviderVenueQualificationFileImportDtoListBuilder()
                .AddVenue(isRemoved: false)
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

            var providerVenueDetailViewModel = new ProviderVenueDetailViewModel
            {
                Id = 1,
                ProviderId = 1,
                Postcode = dto.VenuePostcode,
                Name = dto.VenueName,
                IsEnabledForReferral = dto.VenueIsEnabledForReferral,
                Qualifications = new List<QualificationDetailViewModel>()
            };

            //Use reflection to hack into the IsRemoved property and set it
            var writeableIsRemovedField = typeof(ProviderVenueDetailViewModel)
                            .GetRuntimeFields().FirstOrDefault(a => Regex.IsMatch(a.Name, $"\\A<{nameof(ProviderVenueDetailViewModel.IsRemoved)}>k__BackingField\\Z"));
            if (writeableIsRemovedField == null)
            {
                throw new TypeAccessException("Failed to get write access field for ProviderVenueDetailViewModel.IsRemoved");
            }

            writeableIsRemovedField.SetValue(providerVenueDetailViewModel, true);

            _providerVenueService
                .GetVenueAsync(1, "CV1 2WT")
                .Returns(providerVenueDetailViewModel);

            _qualificationService.GetQualificationAsync(Arg.Any<string>()).Returns((QualificationDetailViewModel)null);
            _providerQualificationService.GetProviderQualificationAsync(Arg.Any<int>(), Arg.Any<int>()).Returns((ProviderQualificationDto)null);
            _routePathService.GetRouteSummaryByNameAsync(Arg.Any<string>()).Returns((RouteSummaryViewModel)null);

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
        public void Then_ProviderVenueService_GetVenueAsync_Is_Called_Exactly_Once()
        {
            _providerVenueService
                .Received(1)
                .GetVenueAsync(1, "CV1 2WT");
        }

        [Fact]
        public void Then_ProviderVenueService_UpdateVenueAsync_Is_Not_Called_To_Remove_Venue()
        {
            _providerVenueService
                .DidNotReceive()
                .UpdateVenueAsync(Arg.Any<RemoveProviderVenueViewModel>());
        }

        [Fact]
        public void Then_ProviderVenueService_UpdateVenueToNotRemovedAsync_Is_Called_Exactly_Once()
        {
            _providerVenueService
                .Received(1)
                .UpdateVenueToNotRemovedAsync(Arg.Any<RemoveProviderVenueViewModel>());
        }

        [Fact]
        public void Then_ProviderVenueService_UpdateVenueToNotRemovedAsync_Is_Called_To_Unremove_Venue_With_Expected_Values()
        {
            _providerVenueService
                .Received(1)
                .UpdateVenueToNotRemovedAsync(Arg.Is<RemoveProviderVenueViewModel>(
                    p =>
                        p.ProviderId == 1 &&
                        p.ProviderVenueId == 1 &&
                        p.Postcode == "CV1 2WT"));
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
            _providerVenueService.Received(1)
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
    }
}
