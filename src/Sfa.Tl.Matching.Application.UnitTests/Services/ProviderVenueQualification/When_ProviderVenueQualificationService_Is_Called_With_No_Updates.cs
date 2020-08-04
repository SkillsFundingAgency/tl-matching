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
        private readonly IEnumerable<ProviderVenueQualificationUpdateResultsDto> _results;

        public When_ProviderVenueQualificationService_Is_Called_With_No_Updates()
        {
            _providerService = Substitute.For<IProviderService>();
            _providerVenueService = Substitute.For<IProviderVenueService>();
            var providerQualificationService = Substitute.For<IProviderQualificationService>();
            var qualificationService = Substitute.For<IQualificationService>();
            var routePathService = Substitute.For<IRoutePathService>();
            var qualificationRouteMappingService = Substitute.For<IQualificationRouteMappingService>();

            var dtoList = new ValidProviderVenueQualificationFileImportDtoListBuilder()
                .AddVenue()
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

            var providerVenueQualificationService = new ProviderVenueQualificationService
                (
                   _providerService,
                   _providerVenueService,
                   providerQualificationService,
                   qualificationService,
                   routePathService,
                   qualificationRouteMappingService
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
            _providerService.Received(1)
                .SearchAsync(Arg.Any<long>());
        }

        [Fact]
        public void Then_ProviderService_GetProviderDetailByIdAsync_Is_Called_Exactly_Once()
        {
            _providerService.Received(1)
                .GetProviderDetailByIdAsync(Arg.Any<int>());
        }

        [Fact]
        public void Then_ProviderService_UpdateProviderDetailAsync_Is_Not_Called()
        {
            _providerService
                .DidNotReceive()
                .UpdateProviderDetailAsync(Arg.Any<ProviderDetailViewModel>());
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

    }
}
