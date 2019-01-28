using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.Services
{
    public interface IUploadService
    {
        Task Upload(IFormFile file, DataImportViewModel viewModel);
    }
}