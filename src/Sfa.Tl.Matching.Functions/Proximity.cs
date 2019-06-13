using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public class Proximity
    {
        // ReSharper disable once UnusedMember.Global
            ExecutionContext context,
            ILogger logger,
            [Inject] IRepository<FunctionLog> functionlogRepository
        )
        {
            try
            {
            }
            catch (Exception e)
            {

                logger.LogError(errormessage);

                await functionlogRepository.Create(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    RowNumber = -1
                });
                throw;
            }
        }
    }
}