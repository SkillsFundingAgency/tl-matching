using System;
using System.Text.Json;
using Microsoft.Azure.Cosmos.Table;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Application.Configuration
{
    public static class ConfigurationLoader
    {
        public static MatchingConfiguration Load(
            string environment, 
            string storageConnectionString,
            string serviceName,
            string version)
        {
            try
            {
                var conn = CloudStorageAccount.Parse(storageConnectionString);
                var tableClient = conn.CreateCloudTableClient();
                var table = tableClient.GetTableReference("Configuration");

                var operation = TableOperation.Retrieve(environment, $"{serviceName}_{version}");
                var result = table.ExecuteAsync(operation).GetAwaiter().GetResult();

                var dynResult = result.Result as DynamicTableEntity;
                var data = dynResult?.Properties["Data"].StringValue;

                if (data == null)
                {
                    throw new NullReferenceException("Configuration data was null.");
                }

                return JsonSerializer.Deserialize<MatchingConfiguration>(data,
                    new JsonSerializerOptions
                    {
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true
                    });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Configuration could not be loaded. Please check your configuration files or see the inner exception for details", ex);
            }
        }
    }
}