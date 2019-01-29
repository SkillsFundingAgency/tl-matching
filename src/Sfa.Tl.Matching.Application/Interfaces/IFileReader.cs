using System.Collections.Generic;
using System.IO;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IFileReader<out TDto> where TDto : class, new()
    {
        IEnumerable<TDto> ValidateAndParseFile(Stream stream);
    }
}