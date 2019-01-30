using System.IO;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.FileReader.Excel.Employer;
using Sfa.Tl.Matching.Functions.Extensions;
// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Functions
{
    public static class CreateEmployers
    {
        [FunctionName("CreateEmployers")]
        public static void Run(
            [BlobTrigger("files/Employer/{name}", Connection = "AzureWebJobsStorage")]Stream stream, 
            string name, 
            ILogger logger,
            [Inject] IMapper mapper
            )
        {
            logger.LogInformation($"Processing Employer blob\n Name:{name} \n Size: {stream.Length} Bytes");

            // TODO AU DI
            var connectionString = "Server=(local);Database=TL;Trusted_Connection=True;MultipleActiveResultSets=true;";
            var options = new DbContextOptionsBuilder<MatchingDbContext>().UseSqlServer(connectionString).Options;
            var matchingDbContext = new MatchingDbContext(options);
            var repository = new EmployerCommandRepository(logger, matchingDbContext);
            var createEmployerService = new CreateEmployerService(logger,
                mapper,
                new ExcelEmployerFileReader(),
                repository);

            var createdRecords = createEmployerService.Process(stream);

            logger.LogInformation($"Processed {createdRecords} Employer records from blob\n Name:{name} \n Size: {stream.Length} Bytes");
        }
    }
}