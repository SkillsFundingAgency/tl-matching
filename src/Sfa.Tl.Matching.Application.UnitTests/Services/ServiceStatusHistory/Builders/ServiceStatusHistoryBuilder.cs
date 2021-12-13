using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ServiceStatusHistory.Builders
{
    public class ServiceStatusHistoryBuilder
    {
        public IList<Domain.Models.ServiceStatusHistory> Build() => new List<Domain.Models.ServiceStatusHistory>
        {
            new()
            {
                Id = 1,
                IsOnline = false
            }
        };

        public IList<Domain.Models.ServiceStatusHistory> BuildEmptyList() =>
            new List<Domain.Models.ServiceStatusHistory>();
    }
}
