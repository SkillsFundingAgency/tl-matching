using System;
using System.Linq;
using System.Text.Json;
using Azure.Data.Tables;
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
                var tableClient = new TableClient(storageConnectionString, "Configuration");
                var tableEntity = tableClient
                    .Query<TableEntity>(
                        filter: $"PartitionKey eq '{environment}' and RowKey eq '{serviceName}_{version}'");

                var data = tableEntity.FirstOrDefault()?["Data"]?.ToString();

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