using System.Threading.Tasks;
using Sfa.Tl.Matching.Infrastructure.Configuration;

namespace Sfa.Tl.Matching.Application.Services
{
    public interface IConfigurationService
    {
        Task<MatchingConfiguration> GetConfig(string environment, string storageConnectionString,
            string version, string serviceName);
    }
}
