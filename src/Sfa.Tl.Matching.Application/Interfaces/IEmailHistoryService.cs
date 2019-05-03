using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmailHistoryService
    {
        Task SaveEmailHistory(string emailTemplateName, IDictionary<string, string> tokens, int? opportunityId, string emailAddress, string createdBy);
    }
}
