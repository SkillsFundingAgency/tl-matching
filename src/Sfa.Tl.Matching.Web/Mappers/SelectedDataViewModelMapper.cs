using System.IO;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Http;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    // ReSharper disable once UnusedMember.Global
    public class SelectedDataViewModelMapper : MapperConfigurationExpression
    {
        public SelectedDataViewModelMapper()
        {
            CreateMap<IFormFile, SelectedImportDataViewModel>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(source => source.FileName))
                .ForMember(dest => dest.Type, opt => opt.MapFrom((source, dest) => (DataImportType) dest.Id))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(source => source.ContentType))
                .ForMember(dest => dest.Data, opt => opt.MapFrom(source => GetByteArray(source)))
                ;
        }

        private static byte[] GetByteArray(IFormFile source)
        {
            using (var ms = new MemoryStream())
            {
                source.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}