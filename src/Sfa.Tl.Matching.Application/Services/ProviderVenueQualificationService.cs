using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using System.Collections.Generic;
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

        public async Task Update(IEnumerable<ProviderVenueQualification> data)
        {
            foreach (var providerVenueQualification in data)
            {
                if (providerVenueQualification.InMatchingService)
                {
                    var provider = await _providerService.SearchAsync(providerVenueQualification.UkPrn);

                    if (provider == null)
                    {
                        continue;
                    }

                    var providerViewModel = await _providerService.GetProviderDetailByIdAsync(provider.Id);

                    if (providerViewModel != null)
                    {
                        var providerValidator = ValidateProviderToUpdate(providerViewModel, providerVenueQualification);

                        if (providerValidator.IsUpdated)
                        {
                            await _providerService.UpdateProviderDetailAsync(providerValidator.ProviderDetailViewModel);
                        }

                        // Provider Venue Update
                        var venueViewModel = await _providerVenueService.GetVenueAsync(provider.Id, providerVenueQualification.VenuePostcode); 

                        if (venueViewModel == null)
                        {
                            var addProviderVenue = new AddProviderVenueViewModel()
                            {
                                ProviderId = provider.Id,
                                Postcode = providerVenueQualification.VenuePostcode,
                                Source = Source
                            };

                            var venueId = await _providerVenueService.CreateVenueAsync(addProviderVenue);
                        }

                        // Provider Venue Delete
                        if (providerVenueQualification.VenueIsRemoved && venueViewModel != null)
                        {
                            var removeProviderVenueViewModel = new RemoveProviderVenueViewModel()
                            {
                                Postcode = venueViewModel.Postcode,
                                ProviderId = venueViewModel.ProviderId,
                                ProviderVenueId = venueViewModel.Id
                            };

                            await _providerVenueService.UpdateVenueAsync(removeProviderVenueViewModel);
                        }

                        // Qualification
                        if (!providerVenueQualification.VenueIsRemoved)
                        {
                            var qualification = await _qualificationService.GetQualificationAsync(providerVenueQualification.LarId);

                            int qualificationId = 0;

                            if (qualification == null)
                            {
                                var missingQualificationViewModel = new MissingQualificationViewModel()
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
                            if (providerVenueQualification.QualificationIsDeleted && providerQualificationViewModel != null)
                            {
                                var removeProviderQualificationViewModel = new RemoveProviderQualificationViewModel()
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
                                var addQualificationViewModel = new AddQualificationViewModel()
                                {
                                    LarId = providerVenueQualification.LarId,
                                    QualificationId = qualification.Id,
                                    Source = Source,
                                    ProviderVenueId = venueViewModel.Id,
                                    Postcode = venueViewModel.Postcode
                                };

                                await _providerQualificationService.CreateProviderQualificationAsync(addQualificationViewModel);
                            }

                            // Route Mapping
                            var route = await _routePathService.GetRouteSummaryByNameAsync(providerVenueQualification.Route);
                            var routeMapping = await _qualificationRouteMappingService.GetQualificationRouteMappingAsync(route.Id, qualificationId);

                            if (routeMapping == null)
                            {
                                var qualificationRouteMappingViewModel = new QualificationRouteMappingViewModel()
                                {
                                    RouteId = route.Id,
                                    QualificationId = qualificationId
                                };

                                await _qualificationRouteMappingService.CreateQualificationRouteMappingAsync(qualificationRouteMappingViewModel);
                            }
                        }

                    }

                }
            }
        }

        public (bool IsUpdated, ProviderDetailViewModel ProviderDetailViewModel) ValidateProviderToUpdate(ProviderDetailViewModel providerDetailViewModel, ProviderVenueQualification providerVenueQualification)
        {
            bool isUpdated = false;

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

        public bool ValidateToUpdate(string valueToUpdate, string value)
        {
            if (!string.IsNullOrEmpty(valueToUpdate) && valueToUpdate != value)
            {
                return true;
            }

            return false;
        }
    }
}
