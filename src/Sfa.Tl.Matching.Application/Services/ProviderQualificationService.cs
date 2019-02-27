using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderQualificationService : IProviderQualificationService
    {
        private readonly IMapper _mapper;
        private readonly IFileReader<ProviderQualificationFileImportDto, ProviderQualificationDto> _fileReader;
        private readonly IRepository<ProviderQualification> _repository;

        public ProviderQualificationService(
            IMapper mapper,
            IFileReader<ProviderQualificationFileImportDto, ProviderQualificationDto> fileReader,
            IRepository<ProviderQualification> repository)
        {
            _mapper = mapper;
            _fileReader = fileReader;
            _repository = repository;
        }
    }
}