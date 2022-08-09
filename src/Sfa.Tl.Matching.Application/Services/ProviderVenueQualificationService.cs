using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderVenueQualificationService : IProviderVenueQualificationService
    {
        private readonly IProviderService _providerService;
        private readonly IProviderVenueService _providerVenueService;
        private readonly IProviderQualificationService _providerQualificationService;
        private readonly IQualificationService _qualificationService;
        private readonly IRoutePathService _routePathService;
        private readonly IQualificationRouteMappingService _qualificationRouteMappingService;

        private const string Source = "Import";

        public ProviderVenueQualificationService(IProviderService providerService,
            IProviderVenueService providerVenueService,
            IProviderQualificationService providerQualificationService,
            IQualificationService qualificationService,
            IRoutePathService routePathService,
            IQualificationRouteMappingService qualificationRouteMappingService)
        {
            _providerService = providerService;
            _providerVenueService = providerVenueService;
            _providerQualificationService = providerQualificationService;
            _qualificationService = qualificationService;
            _routePathService = routePathService;
            _qualificationRouteMappingService = qualificationRouteMappingService;
        }

        public async Task<IEnumerable<ProviderVenueQualificationUpdateResultsDto>> UpdateAsync(IEnumerable<ProviderVenueQualificationDto> data)
        {
            var results = new List<ProviderVenueQualificationUpdateResultsDto>();

            foreach (var providerVenueQualification in data)
            {
                var result = new ProviderVenueQualificationUpdateResultsDto
                {
                    UkPrn = providerVenueQualification.UkPrn,
                    VenuePostcode = providerVenueQualification.VenuePostcode,
                    LarId = providerVenueQualification.LarId,
                    Message = $"UkPrn: {providerVenueQualification.UkPrn} - Data import successful for Provider Name: {providerVenueQualification.ProviderName}"
                };

                try
                {
                    var providerId = await CreateOrUpdateProviderAsync(providerVenueQualification);

                    var venueViewModel = await CreateOrUpdateProviderVenueAsync(providerVenueQualification, providerId);

                    // Qualification
                    if (venueViewModel != null && !string.IsNullOrWhiteSpace(providerVenueQualification.LarId))
                    {
                        var qualificationId = await CreateOrUpdateQualificationAsync(providerVenueQualification, venueViewModel.Id);

                        await CreateOrUpdateProviderQualificationAsync(providerVenueQualification, venueViewModel.Id, venueViewModel.Postcode, qualificationId);

                        // Route Mapping
                        var routeErrors = await CreateOrUpdateQualificationRouteMappingAsync(providerVenueQualification, qualificationId);
                        if (!string.IsNullOrEmpty(routeErrors))
                        {
                            result.HasErrors = true;
                            result.Message = routeErrors;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.HasErrors = true;
                    result.Message = $"Import failed for UkPrn: {providerVenueQualification.UkPrn}, postcode: {providerVenueQualification.VenuePostcode}, LAR Id: {providerVenueQualification.LarId} \n Error Message: {ex.Message}\n StackTrace: {ex.StackTrace}";
                }

                results.Add(result);
            }

            return results;
        }

        private async Task<int> CreateOrUpdateProviderAsync(ProviderVenueQualificationDto providerVenueQualification)
        {
            int providerId;
            var provider = await _providerService.SearchAsync(providerVenueQualification.UkPrn);

            if (provider == null)
            {
                //Create the new provider
                var createProviderViewModel = new CreateProviderDetailViewModel
                {
                    UkPrn = providerVenueQualification.UkPrn,
                    Name = providerVenueQualification.Name,
                    DisplayName = !string.IsNullOrWhiteSpace(providerVenueQualification.DisplayName)
                        ? providerVenueQualification.DisplayName
                        : providerVenueQualification.Name,
                    PrimaryContact = providerVenueQualification.PrimaryContact,
                    PrimaryContactEmail = providerVenueQualification.PrimaryContactEmail,
                    PrimaryContactPhone = providerVenueQualification.PrimaryContactPhone,
                    SecondaryContact = providerVenueQualification.SecondaryContact,
                    SecondaryContactEmail = providerVenueQualification.SecondaryContactEmail,
                    SecondaryContactPhone = providerVenueQualification.SecondaryContactPhone,
                    IsEnabledForReferral = providerVenueQualification.IsEnabledForReferral,
                    IsCdfProvider = providerVenueQualification.IsCdfProvider,
                    //NOTE: IsTLevelProvider needs to be set manually
                    Source = Source
                };

                providerId = await _providerService.CreateProviderAsync(createProviderViewModel);
                Debug.WriteLine($"Created provider with id {providerId} for UKPRN {providerVenueQualification.UkPrn}");
            }
            else
            {
                //Update an existing provider
                providerId = provider.Id;

                var providerViewModel = await _providerService.GetProviderDetailByIdAsync(providerId);
                if (providerViewModel != null)
                {
                    var providerValidator =
                        ValidateProviderToUpdate(providerViewModel, providerVenueQualification);

                    if (providerValidator.IsUpdated)
                    {
                        await _providerService.UpdateProviderDetailAsync(providerValidator
                            .ProviderDetailViewModel);
                    }
                }
            }

            return providerId;
        }

        private async Task<ProviderVenueDetailViewModel> CreateOrUpdateProviderVenueAsync(ProviderVenueQualificationDto providerVenueQualification, int providerId)
        {
            if (string.IsNullOrEmpty(providerVenueQualification.VenuePostcode))
            {
                return null;
            }

            var venueViewModel = await _providerVenueService.GetVenueWithTrimmedPostcodeAsync(providerId,
                        providerVenueQualification.VenuePostcode);

            if (venueViewModel == null)
            {
                var addProviderVenue = new AddProviderVenueViewModel
                {
                    ProviderId = providerId,
                    Postcode = providerVenueQualification.VenuePostcode,
                    Source = Source
                };

                var venueId = await _providerVenueService.CreateVenueAsync(addProviderVenue);
                Debug.WriteLine(
                    $"Created venue with id {venueId} for provider {providerId} and postcode {providerVenueQualification.VenuePostcode}");

                venueViewModel = await _providerVenueService.GetVenueAsync(venueId);
            }
            else if (venueViewModel.IsRemoved != providerVenueQualification.VenueIsRemoved)
            {
                venueViewModel.IsRemoved = providerVenueQualification.VenueIsRemoved;
            }

            var providerVenueValidator = ValidateProviderVenueToUpdate(venueViewModel, providerVenueQualification);

            if (providerVenueValidator.IsUpdated)
            {
                await _providerVenueService.UpdateVenueAsync(providerVenueValidator.ProviderVenueDetailViewModel);
            }

            return venueViewModel;
        }

        private async Task<int> CreateOrUpdateQualificationAsync(ProviderVenueQualificationDto providerVenueQualification, int providerVenueId)
        {
            var qualification = await _qualificationService.GetQualificationAsync(providerVenueQualification.LarId);

            int qualificationId;

            if (qualification == null)
            {
                var missingQualificationViewModel = new MissingQualificationViewModel
                {
                    LarId = providerVenueQualification.LarId,
                    ProviderVenueId = providerVenueId,
                    Title = providerVenueQualification.QualificationTitle,
                    ShortTitle = providerVenueQualification.QualificationShortTitle,
                    Source = Source
                };

                qualificationId = await _qualificationService.CreateQualificationEntityAsync(missingQualificationViewModel);
                Debug.WriteLine($">>>> Created Removing Qualification {qualificationId}");
            }
            else
            {
                qualificationId = qualification.Id;
            }

            return qualificationId;
        }

        private async Task CreateOrUpdateProviderQualificationAsync(ProviderVenueQualificationDto providerVenueQualification,
            int providerVenueId, string postcode, int qualificationId)
        {
            var providerQualificationViewModel =
                await _providerQualificationService.GetProviderQualificationAsync(providerVenueId, qualificationId);

            if (providerQualificationViewModel == null && providerVenueQualification.QualificationIsOffered)
            {
                var addQualificationViewModel = new AddQualificationViewModel
                {
                    LarId = providerVenueQualification.LarId,
                    QualificationId = qualificationId,
                    Source = Source,
                    ProviderVenueId = providerVenueId,
                    Postcode = postcode
                };

                Debug.WriteLine(
                    $">>>> Creating ProviderQualification {addQualificationViewModel.LarId} - {addQualificationViewModel.QualificationId} - {addQualificationViewModel.ProviderVenueId}");
                await _providerQualificationService.CreateProviderQualificationAsync(addQualificationViewModel);
            }
            // Delete Provider Venue Qualification
            else if (providerQualificationViewModel != null && !providerVenueQualification.QualificationIsOffered)
            {
                Debug.WriteLine(
                    $">>>> Removing ProviderQualification {providerVenueId} - {qualificationId} ({providerQualificationViewModel.ProviderVenueId} - {providerQualificationViewModel.QualificationId})");
                await _providerQualificationService.RemoveProviderQualificationAsync(providerVenueId, qualificationId);
            }
        }

        private async Task<string> CreateOrUpdateQualificationRouteMappingAsync(ProviderVenueQualificationDto providerVenueQualification,
            int qualificationId)
        {
            var invalidRouteNames = new List<string>();

            foreach (var routeName in providerVenueQualification.Routes)
            {
                var route = await _routePathService.GetRouteSummaryByNameAsync(routeName);

                if (route == null)
                {
                    invalidRouteNames.Add($"{routeName}");
                    continue;
                }

                var routeMapping =
                    await _qualificationRouteMappingService.GetQualificationRouteMappingAsync(route.Id,
                        qualificationId);

                if (routeMapping == null)
                {
                    var qualificationRouteMappingViewModel = new QualificationRouteMappingViewModel
                    {
                        RouteId = route.Id,
                        QualificationId = qualificationId,
                        Source = Source
                    };

                    await _qualificationRouteMappingService.CreateQualificationRouteMappingAsync(
                        qualificationRouteMappingViewModel);
                }
            }

            if (invalidRouteNames.Any())
            {
                var sb = new StringBuilder($"Data Error: Route{(invalidRouteNames.Count > 1 ? "s" : "")} ");

                for (var i = 0; i < invalidRouteNames.Count; i++)
                {
                    sb.Append($"{(i > 0 ? ", " : "")}'{invalidRouteNames[i]}'");
                }

                sb.Append($" not found in existing Routes. UkPrn: {providerVenueQualification.UkPrn}, postcode: {providerVenueQualification.VenuePostcode}, LAR Id: {providerVenueQualification.LarId}");

                return sb.ToString();
            }

            return null;
        }

        private static (bool IsUpdated, ProviderDetailViewModel ProviderDetailViewModel) ValidateProviderToUpdate(ProviderDetailViewModel providerDetailViewModel, ProviderVenueQualificationDto providerVenueQualification)
        {
            var isUpdated = false;

            if (ValidateToUpdate(providerDetailViewModel.Name, providerVenueQualification.Name))
            {
                providerDetailViewModel.Name = providerVenueQualification.Name;
                isUpdated = true;
            }

            if (ValidateToUpdate(providerDetailViewModel.DisplayName, providerVenueQualification.DisplayName))
            {
                providerDetailViewModel.DisplayName = providerVenueQualification.DisplayName;
                isUpdated = true;
            }

            if (ValidateToUpdate(providerDetailViewModel.PrimaryContact, providerVenueQualification.PrimaryContact))
            {
                providerDetailViewModel.PrimaryContact = providerVenueQualification.PrimaryContact;
                isUpdated = true;
            }

            if (ValidateToUpdate(providerDetailViewModel.PrimaryContactEmail, providerVenueQualification.PrimaryContactEmail))
            {
                providerDetailViewModel.PrimaryContactEmail = providerVenueQualification.PrimaryContactEmail;
                isUpdated = true;
            }

            if (ValidateToUpdate(providerDetailViewModel.PrimaryContactPhone, providerVenueQualification.PrimaryContactPhone))
            {
                providerDetailViewModel.PrimaryContactPhone = providerVenueQualification.PrimaryContactPhone;
                isUpdated = true;
            }

            if (ValidateToUpdate(providerDetailViewModel.SecondaryContact, providerVenueQualification.SecondaryContact))
            {
                providerDetailViewModel.SecondaryContact = providerVenueQualification.SecondaryContact;
                isUpdated = true;
            }

            if (ValidateToUpdate(providerDetailViewModel.SecondaryContactEmail, providerVenueQualification.SecondaryContactEmail))
            {
                providerDetailViewModel.SecondaryContactEmail = providerVenueQualification.SecondaryContactEmail;
                isUpdated = true;
            }

            if (ValidateToUpdate(providerDetailViewModel.SecondaryContactPhone, providerVenueQualification.SecondaryContactPhone))
            {
                providerDetailViewModel.SecondaryContactPhone = providerVenueQualification.SecondaryContactPhone;
                isUpdated = true;
            }

            if (!providerDetailViewModel.IsEnabledForReferral.HasValue
                || providerDetailViewModel.IsEnabledForReferral.Value != providerVenueQualification.IsEnabledForReferral)
            {
                providerDetailViewModel.IsEnabledForReferral = providerVenueQualification.IsEnabledForReferral;
                isUpdated = true;
            }

            if (providerDetailViewModel.IsCdfProvider != providerVenueQualification.IsCdfProvider)
            {
                providerDetailViewModel.IsCdfProvider = providerVenueQualification.IsCdfProvider;
                isUpdated = true;
            }

            return (isUpdated, providerDetailViewModel);
        }

        private static (bool IsUpdated, ProviderVenueDetailViewModel ProviderVenueDetailViewModel) ValidateProviderVenueToUpdate(ProviderVenueDetailViewModel providerVenueDetailViewModel, ProviderVenueQualificationDto providerVenueQualification)
        {
            var isUpdated = false;

            if (ValidateToUpdate(providerVenueDetailViewModel.Name, providerVenueQualification.VenueName))
            {
                providerVenueDetailViewModel.Name = providerVenueQualification.VenueName;
                isUpdated = true;
            }

            if (providerVenueDetailViewModel.IsEnabledForReferral != providerVenueQualification.VenueIsEnabledForReferral)
            {
                providerVenueDetailViewModel.IsEnabledForReferral = providerVenueQualification.VenueIsEnabledForReferral;
                isUpdated = true;
            }

            return (isUpdated, providerVenueDetailViewModel);
        }

        private static bool ValidateToUpdate(string oldValue, string newValue)
        {
            return !string.IsNullOrEmpty(newValue) && oldValue != newValue;
        }
    }
}
