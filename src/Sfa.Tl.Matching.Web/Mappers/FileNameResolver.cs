using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class FileNameResolver : IValueResolver<DataImportParametersViewModel, DataImportDto, string>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public FileNameResolver(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public string Resolve(DataImportParametersViewModel source, DataImportDto dest, string destMember, ResolutionContext context)
        {
            return $"{_dateTimeProvider.UtcNowString("yyyyMMddHHmmssfffffffK")}{source.File.FileName}";
        }
    }
}