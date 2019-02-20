using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class DataImportViewModelMapper : Profile
    {
        public DataImportViewModelMapper()
        {
            CreateMap<DataImportParametersViewModel, DataUploadDto>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom<FileNameResolver>())
                .ForMember(dest => dest.ImportType, opt => opt.MapFrom(source => source.SelectedImportType))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(source => source.File.ContentType))
                .ForMember(dest => dest.Data, opt => opt.MapFrom(source => GetByteArray(source.File)))
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
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