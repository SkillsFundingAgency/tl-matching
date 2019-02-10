using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IDataParser<out T> where T : class
    {
        IEnumerable<T> Parse(FileImportDto dto);
    }
}