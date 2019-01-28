using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.FileReader.Excel.Employer;
using Sfa.Tl.Matching.Models;

namespace Sfa.Tl.Matching.Application.Services
{
    public class CreateEmployerService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IEmployerFileReader _fileReader;
        private readonly IEmployerCommandRepository _repository;

        public CreateEmployerService(ILogger logger,
            IMapper mapper,
            IEmployerFileReader fileReader,
            IEmployerCommandRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _fileReader = fileReader;
            _repository = repository;
        }

        public async Task<int> Process(Stream stream)
        {
            var result = _fileReader.Load(stream);

            if (!string.IsNullOrEmpty(result.Error))
                _logger.LogError(result.Error);

            var createEmployerDtos = _mapper.Map<List<CreateEmployerDto>>(result.Data);
            //var employers = _mapper.Map<List<Domain.Models.Employer>>(createEmployerDtos);
            var employers = Map(createEmployerDtos);

            if (employers.Count == 0)
            {
                _logger.LogWarning("No employers to add to the database, so quitting");
                return 0;
            }

            await _repository.ResetData();
            var createdRecords = await _repository.CreateMany(employers);

            return createdRecords;
        }

        #region Private Methods
        private static List<Domain.Models.Employer> Map(List<CreateEmployerDto> createEmployerDtos)
        {
            var employers = createEmployerDtos.Select(edto => new Domain.Models.Employer()
            {
                CrmId = edto.CrmId,
                AlsoKnownAs = edto.AlsoKnownAs,
                CompanyName = edto.CompanyName,
                Email = edto.Email,
                Owner = edto.Owner,
                Phone = edto.Phone,
                PostCode = edto.PostCode,
                PrimaryContact = edto.PrimaryContact
            }).ToList();

            return employers;
        }
        #endregion
    }
}