﻿using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQuarterlyUpdateEmailService.Builders
{
    public class ValidProviderWithFundingDtoListBuilder
    {
        public IList<ProviderWithFundingDto> Build() => new List<ProviderWithFundingDto>
        {
            new()
            {
                Id = 1,
                Name = "Provider Name",
                PrimaryContact = "Provider Contact",
                PrimaryContactEmail = "primary.contact@provider.co.uk",
                PrimaryContactPhone = "01777757777",
                SecondaryContact = "SecondaryContact",
                SecondaryContactEmail = "secondary@contact.co.uk",
                SecondaryContactPhone = "01234559999",
                ProviderVenues = new List<ProviderVenueQualificationsInfoDto>
                {
                    new()
                    {
                        Postcode = "AA1 1AA",
                        Qualifications = new List<QualificationInfoDto>
                        {
                            new()
                            {
                                LarId = "10042982",
                                Title = "Qualification 1"
                            },
                            new()
                            {
                                LarId = "60165522",
                                Title = "Qualification 2"
                            }
                        }
                    }
                }
            }
        };

        public IList<ProviderWithFundingDto> BuildWithNoQualifications() => new List<ProviderWithFundingDto>
        {
            new()
            {
                Id = 1,
                Name = "Provider Name",
                PrimaryContact = "Provider Contact",
                PrimaryContactEmail = "primary.contact@provider.co.uk",
                PrimaryContactPhone = "01777757777",
                SecondaryContact = "SecondaryContact",
                SecondaryContactEmail = "secondary@contact.co.uk",
                SecondaryContactPhone = "01234559999",
                ProviderVenues = new List<ProviderVenueQualificationsInfoDto>
                {
                    new()
                    {
                        Postcode = "AA1 1AA",
                        Qualifications = new List<QualificationInfoDto>()
                    }
                }
            }
        };

        public IList<ProviderWithFundingDto> BuildWithNoVenues() => new List<ProviderWithFundingDto>
        {
            new()
            {
                Id = 1,
                Name = "Provider Name",
                PrimaryContact = "Provider Contact",
                PrimaryContactEmail = "primary.contact@provider.co.uk",
                PrimaryContactPhone = "01777757777",
                SecondaryContact = "SecondaryContact",
                SecondaryContactEmail = "secondary@contact.co.uk",
                SecondaryContactPhone = "01234559999",
                ProviderVenues = new List<ProviderVenueQualificationsInfoDto>()
            }
        };
    }
}
