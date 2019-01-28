using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.Commands.CreateEmployer
{
    public class CreateEmployerCommand : ICreateEmployerCommand
    {
        private readonly MatchingDbContext _matchingDbContext;

        public CreateEmployerCommand(MatchingDbContext matchingDbContext)
        {
            _matchingDbContext = matchingDbContext;
        }

        public async Task Execute(Employer employer)
        {
            _matchingDbContext.Employer.Add(employer);

            int createdRecordCount;
            try
            {
                createdRecordCount = await _matchingDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                // Log
                throw;
            }
        }
    }
}