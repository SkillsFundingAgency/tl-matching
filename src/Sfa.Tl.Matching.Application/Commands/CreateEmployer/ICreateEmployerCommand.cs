using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.Commands.CreateEmployer
{
    public interface ICreateEmployerCommand
    {
        Task Execute(Employer employer);
    }
}