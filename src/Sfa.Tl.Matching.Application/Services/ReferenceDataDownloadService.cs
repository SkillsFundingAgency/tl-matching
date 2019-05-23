using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.UkRlp.Api.Client;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ReferenceDataDownloadService : IReferenceDataDownloadService
    {
        private readonly IProviderDownload _providerDownload;
        private readonly IRepository<ProviderReferenceStaging> _providerReferenceStagingRepository;

        public ReferenceDataDownloadService(IProviderDownload providerDownload,
            IRepository<ProviderReferenceStaging> providerReferenceStagingRepository)
        {
            _providerDownload = providerDownload;
            _providerReferenceStagingRepository = providerReferenceStagingRepository;
        }


    }
}