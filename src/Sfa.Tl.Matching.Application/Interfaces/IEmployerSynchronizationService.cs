﻿using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmployerSynchronizationService
    {
        Task<int> SynchronizeEmployers(EmployerStagingFileImportDto learningAimsReferenceStagingFileImportDto);
    }
}