using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IDataValidator
    {
        bool Validate(SharedStringTablePart stringTablePart, IEnumerable<Cell> cells);
    }
}