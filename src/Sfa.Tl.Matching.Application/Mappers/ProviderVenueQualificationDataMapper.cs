using CsvHelper.Configuration;
using Sfa.Tl.Matching.Domain.Models;

namespace sfa.Tl.Marketing.Communication.DataLoad.Read
{
    internal sealed class ProviderVenueQualificationDataMapper : ClassMap<ProviderVenueQualification>
    {
        internal ProviderVenueQualificationDataMapper()
        {
            Map(m => m.UkPrn).Name("UKPRN");
            Map(m => m.InMatchingService).Name("In Matching Service")
                .TypeConverterOption.BooleanValues(true, true, "Yes")
                .TypeConverterOption.BooleanValues(false, true, "No", "-");
            Map(m => m.ProviderName).Name("Provider Name");
            Map(m => m.Name).Name("Name");
            Map(m => m.DisplayName).Name("DisplayName");
            Map(m => m.IsCDFProvider).Name("IsCDFProvider")
                .TypeConverterOption.BooleanValues(true, true, "TRUE")
                .TypeConverterOption.BooleanValues(false, true, "FALSE", "-");
            Map(m => m.IsEnabledForReferral).Name("IsEnabledForReferral")
                .TypeConverterOption.BooleanValues(true, true, "TRUE")
                .TypeConverterOption.BooleanValues(false, true, "FALSE", "-");

            Map(m => m.PrimaryContact).Name("PrimaryContact");
            Map(m => m.PrimaryContactEmail).Name("PrimaryContactEmail");
            Map(m => m.PrimaryContactPhone).Name("PrimaryContactPhone");

            Map(m => m.SecondaryContact).Name("SecondaryContact");
            Map(m => m.SecondaryContactEmail).Name("SecondaryContactEmail");
            Map(m => m.SecondaryContactPhone).Name("SecondaryContactPhone");

            Map(m => m.VenuePostcode).Name("Venue Postcode");
            Map(m => m.Town).Name("Town");
            Map(m => m.VenueName).Name("Venue Name");

            Map(m => m.VenueIsEnabledForReferral).Name("Venue IsEnabledForReferral")
                .TypeConverterOption.BooleanValues(true, true, "TRUE")
                .TypeConverterOption.BooleanValues(false, true, "FALSE", "-");

            Map(m => m.VenueIsRemoved).Name("Venue IsRemoved")
                .TypeConverterOption.BooleanValues(true, true, "TRUE")
                .TypeConverterOption.BooleanValues(false, true, "FALSE", "-");

            Map(m => m.LarId).Name("LarId");

            Map(m => m.QualificationTitle).Name("Qualification Title");

            Map(m => m.QualificationShortTitle).Name("Qualification Short Title");

            Map(m => m.QualificationIsDeleted).Name("Qualification IsDeleted")
                .TypeConverterOption.BooleanValues(true, true, "TRUE")
                .TypeConverterOption.BooleanValues(false, true, "FALSE", "-");

            Map(m => m.Route).Name("Route");
        }
    }
}
