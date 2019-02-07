using System.Collections.Generic;
using System.IO;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IDataImportService<out TDto>  where TDto : class, new()
    {
        IEnumerable<TDto> Import(Stream stream, DataImportType dataImportType, int headerRows);
    }
}