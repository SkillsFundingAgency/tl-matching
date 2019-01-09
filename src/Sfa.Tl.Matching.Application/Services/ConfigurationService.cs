using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Infrastructure.Configuration;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public async Task<MatchingConfiguration> GetConfig(string environment, string storageConnectionString,
            string version, string serviceName)
        {
            try
            {
                var conn = CloudStorageAccount.Parse(storageConnectionString);
                var tableClient = conn.CreateCloudTableClient();
                var table = tableClient.GetTableReference("Configuration");

                var operation = TableOperation.Retrieve(environment, $"{serviceName}_{version}");
                var result = await table.ExecuteAsync(operation);

                var dynResult = result.Result as DynamicTableEntity;
                var data = dynResult?.Properties["Data"].StringValue;

                return JsonConvert.DeserializeObject<MatchingConfiguration>(data);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Configuration could not be loaded. Please check your configuration files or see the inner exception for details", ex);
            }
        }
    }
}
