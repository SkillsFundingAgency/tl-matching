using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Builders
{
    internal class ValidEmployerBuilder
    {
        private readonly EmployerFileImportDto _provider;

        public ValidEmployerBuilder()
        {
            _provider = new EmployerFileImportDto();
        }

        public EmployerFileImportDto Build() => _provider;
    }
}