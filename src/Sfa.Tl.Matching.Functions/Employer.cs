using System.IO;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    // ReSharper disable UnusedMember.Global
    public static class Employer
    {
        [FunctionName("ImportEmployer")]
        public static void ImportEmployer(
            [BlobTrigger("employer/{name}", Connection = "BlobStorageConnectionString")]Stream stream, 
            string name, 
            ILogger logger,
            [Inject] IMapper mapper,
            [Inject] IEmployerService employerService
            )
        {
            logger.LogInformation($"Processing Employer blob\n Name:{name} \n Size: {stream.Length} Bytes");

            // TODO AU DI
            var connectionString = "Server=(local);Database=TL;Trusted_Connection=True;MultipleActiveResultSets=true;";
            var options = new DbContextOptionsBuilder<MatchingDbContext>().UseSqlServer(connectionString).Options;
            var matchingDbContext = new MatchingDbContext(options);

            //var dataImportService = new DataImportService<EmployerDto, Domain.Models.Employer>(logger, mapper,
            //    new ExcelFileReader<EmployerDto>(new EmployerDataParser(), new EmployerDataValidator()),
            //    new EmployerRepository(logger, matchingDbContext));

            //var createdRecords = dataImportService.Import(stream, DataImportType.Employer);

            //logger.LogInformation($"Processed {createdRecords} Employer records from blob\n Name:{name} \n Size: {stream.Length} Bytes");

            employerService.ImportEmployer();
        }
    }
}