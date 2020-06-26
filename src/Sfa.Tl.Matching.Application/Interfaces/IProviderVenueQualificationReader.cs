using Sfa.Tl.Matching.Application.FileReader.ProviderVenueQualification;
using System.IO;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderVenueQualificationReader
    {
        ProviderVenueQualificationReadResult ReadData(Stream stream);
    }
}
