using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.FileReader.ProviderVenue
{
    public class ProviderVenueValidator : IDataValidator
    {
        private readonly IRepository<Provider> _repository;

        public ProviderVenueValidator(IRepository<Provider> repository)
        {
            _repository = repository;
        }

        public bool Validate(SharedStringTablePart stringTablePart, IEnumerable<Cell> cells)
        {
            //HasNumberOfColumns() = int
            //IsColumnMandatory() = array of true\false
            //ColumnDataType() = array of dataType

            return true;
        }

        private bool IsProviderUkprnValid(string ukprn)
        {
            var provider = _repository.GetSingleOrDefault(prv => prv.Ukprn == ukprn);
            return provider != null;
        }

        private bool HasValidNumberOfColumns(int numberOfColumns, IEnumerable<Cell> cells)
        {
            return cells.Count() == 5;
        }
    }
}