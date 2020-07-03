using Sfa.Tl.Matching.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderVenueQualificationService
    {
        Task Update(IEnumerable<ProviderVenueQualification> data);
    }
}
