# tl-matching


## Configuration

The configuration values will be read from an Azure table. To make this work on a development machine, add a row to the Configuration table in the Azure Storage Explorer with partition key `LOCAL` and RowKey 'Sfa.Tl.Matching_1.0', and with Data populated with the required json.


## Running functions locally 

In order to run the functions project on a development machine, create a local.settings.json file with the following schema and values:

```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "CronSchedule": "0 */15 * * * *",
    "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
    "EnvironmentName": "LOCAL",
    "ServiceName": "Sfa.Tl.Matching",
    "Version": "1.0"
  }
}