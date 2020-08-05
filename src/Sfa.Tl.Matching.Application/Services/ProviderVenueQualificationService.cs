using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<IEnumerable<ProviderVenueQualificationUpdateResultsDto>> Update(IEnumerable<ProviderVenueQualificationDto> data)
        {
            var results = new List<ProviderVenueQualificationUpdateResultsDto>();

            foreach (var providerVenueQualification in data)
            {
                var result = new ProviderVenueQualificationUpdateResultsDto
                {
                    Message = $"UkPrn: {providerVenueQualification.UkPrn} - Data import successful for Provider Name: {providerVenueQualification.ProviderName}"
                };

                try
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

                    // Provider Venue Update
                    var venueViewModel = await _providerVenueService.GetVenueAsync(providerId, providerVenueQualification.VenuePostcode);

                    if (venueViewModel == null)
                    {
                        var addProviderVenue = new AddProviderVenueViewModel
                        {
                            ProviderId = providerId,
                            Postcode = providerVenueQualification.VenuePostcode,
                            Source = Source
                        };

                        var venueId = await _providerVenueService.CreateVenueAsync(addProviderVenue);
                        Debug.WriteLine($"Created venue with id {venueId} for provider {providerId} and postcode {providerVenueQualification.VenuePostcode}");

                        venueViewModel = await _providerVenueService.GetVenueAsync(providerId, providerVenueQualification.VenuePostcode);
                    }
                    else if(venueViewModel.Name != providerVenueQualification.VenueName)
                    {
                        venueViewModel.Name = providerVenueQualification.VenueName;
                        await _providerVenueService.UpdateVenueAsync(venueViewModel);
                    }

                    // Provider Venue Delete
                    if (venueViewModel != null && venueViewModel.IsRemoved != providerVenueQualification.VenueIsRemoved)
                    {
                        var removeProviderVenueViewModel = new RemoveProviderVenueViewModel
                        {
                            Postcode = venueViewModel.Postcode,
                            ProviderId = venueViewModel.ProviderId,
                            ProviderVenueId = venueViewModel.Id
                        };

                        if (providerVenueQualification.VenueIsRemoved)
                            await _providerVenueService.UpdateVenueAsync(removeProviderVenueViewModel);
                        else
                            await _providerVenueService.UpdateVenueToNotRemovedAsync(removeProviderVenueViewModel);
                    }

                    // Qualification
                    if (!string.IsNullOrWhiteSpace(providerVenueQualification.LarId))
                    {
                        //TODO: Not sure we should check VenueIsRemoved
                        if (venueViewModel != null) // && !providerVenueQualification.VenueIsRemoved)
                        {
                            var qualification = await _qualificationService.GetQualificationAsync(providerVenueQualification.LarId);

                            int qualificationId;

                            if (qualification == null)
                            {
                                var missingQualificationViewModel = new MissingQualificationViewModel
                                {
                                    LarId = providerVenueQualification.LarId,
                                    ProviderVenueId = venueViewModel.Id,
                                    Title = providerVenueQualification.QualificationTitle,
                                    ShortTitle = providerVenueQualification.QualificationShortTitle,
                                    Source = Source
                                };

                                qualificationId = await _qualificationService.CreateQualificationEntityAsync(missingQualificationViewModel);
                            }
                            else
                            {
                                qualificationId = qualification.Id;
                            }

                            var providerQualificationViewModel = await _providerQualificationService.GetProviderQualificationAsync(venueViewModel.Id, qualificationId);

                            // Delete Provider Venue Qualification
                            if (!providerVenueQualification.QualificationIsOffered && providerQualificationViewModel != null)
                            {
                                var removeProviderQualificationViewModel = new RemoveProviderQualificationViewModel
                                {
                                    LarId = providerVenueQualification.LarId,
                                    QualificationId = qualificationId,
                                    Source = Source,
                                    ProviderVenueId = venueViewModel.Id,
                                    Postcode = venueViewModel.Postcode
                                };

                                await _providerQualificationService.RemoveProviderQualificationAsync(removeProviderQualificationViewModel);
                            }

                            if (providerQualificationViewModel == null)
                            {
                                var addQualificationViewModel = new AddQualificationViewModel
                                {
                                    LarId = providerVenueQualification.LarId,
                                    QualificationId = qualificationId,
                                    Source = Source,
                                    ProviderVenueId = venueViewModel.Id,
                                    Postcode = venueViewModel.Postcode
                                };

                                await _providerQualificationService.CreateProviderQualificationAsync(addQualificationViewModel);
                            }

                            // Route Mapping
                            foreach (var routeName in providerVenueQualification.Routes)
                            {
                                var route = await _routePathService.GetRouteSummaryByNameAsync(routeName);

                                if (route == null)
                                {
                                    result.HasErrors = true;
                                    result.Message = $"Data Error: Route {routeName} not found in existing Routes.";
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
                                        QualificationId = qualificationId
                                    };

                                    await _qualificationRouteMappingService.CreateQualificationRouteMappingAsync(
                                        qualificationRouteMappingViewModel);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.HasErrors = true;
                    result.Message = $"UkPrn: {providerVenueQualification.UkPrn} Import failed \n Error Message: {ex.Message}\n StackTrace: {ex.StackTrace}";
                    continue;
                }

                results.Add(result);
            }

            return results;
        }

        private (bool IsUpdated, ProviderDetailViewModel ProviderDetailViewModel) ValidateProviderToUpdate(ProviderDetailViewModel providerDetailViewModel, ProviderVenueQualificationDto providerVenueQualification)
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

            if (ValidateToUpdate(providerDetailViewModel.SecondaryContact, providerVenueQualification.SecondaryContact))
            {
                providerDetailViewModel.SecondaryContact = providerVenueQualification.SecondaryContact;
                isUpdated = true;
            }

            return (isUpdated, providerDetailViewModel);
        }
        private bool ValidateToUpdate(string oldValueToUpdate, string newValue)
        {
            return !string.IsNullOrEmpty(newValue) && oldValueToUpdate != newValue;
        }
    }
}
