using System.IO;
using System.Linq;
using CsvHelper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.ProviderVenueQualification
{
    public class ProviderVenueQualificationReader : IProviderVenueQualificationReader
    {
        private const string FailedToImportMessage = "Failed to load CSV file. Please check the format.";

        public ProviderVenueQualificationReadResult ReadData(Stream fileStream)
        {
            var providerVenueQualificationReadResult = new ProviderVenueQualificationReadResult();
            using (var reader = new StreamReader(fileStream))
            using (var csv = new CsvReader(reader))
            {
                try
                {
                    csv.Configuration.RegisterClassMap<ProviderVenueQualificationDataMapper>();
                    var records = csv.GetRecords<ProviderVenueQualificationDto>().ToList();
                    providerVenueQualificationReadResult.Qualifications = records;
                }
                catch (ReaderException re)
                {
                    providerVenueQualificationReadResult.Error = $"{FailedToImportMessage} {re.Message} {re.InnerException?.Message}";
                }
                catch (ValidationException ve)
                {
                    providerVenueQualificationReadResult.Error = $"{FailedToImportMessage} {ve.Message} {ve.InnerException?.Message}";
                }
                catch (BadDataException bde)
                {
                    providerVenueQualificationReadResult.Error = $"{FailedToImportMessage} {bde.Message} {bde.InnerException?.Message}";
                }
            }

            return providerVenueQualificationReadResult;
        }
    }
}