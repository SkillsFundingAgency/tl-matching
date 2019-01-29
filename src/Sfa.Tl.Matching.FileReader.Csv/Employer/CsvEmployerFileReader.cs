//using System.IO;
//using System.Linq;
//using CsvHelper;

//namespace Sfa.Tl.Matching.FileReader.Csv.Employer
//{
//    public class CsvEmployerFileReader : IEmployerFileReader
//    {
//        private const string FailedToImportMessage = "Failed to load Employer file. Please check the format.";

//        public EmployerLoadResult Load(Stream stream)
//        {
//            var textReader = new StreamReader(stream);
//            var csv = new CsvReader(textReader);
//            csv.Configuration.RegisterClassMap<EmployerCsvMap>();

//            var fileLoadResult = new EmployerLoadResult();
//            try
//            {
//                var fileData = csv.GetRecords<FileUploadEmployer>().ToList();
//                fileLoadResult.Data = fileData;
//            }
//            catch (ReaderException re)
//            {
//                fileLoadResult.Error = $"{FailedToImportMessage} {re.Message} {re.InnerException?.Message}";
//            }
//            catch (ValidationException ve)
//            {
//                fileLoadResult.Error = $"{FailedToImportMessage} {ve.Message} {ve.InnerException?.Message}";
//            }
//            catch (BadDataException bde)
//            {
//                fileLoadResult.Error = $"{FailedToImportMessage} {bde.Message} {bde.InnerException?.Message}";
//            }

//            return fileLoadResult;
//        }
//    }
//}