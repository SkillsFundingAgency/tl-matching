using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IDataBlobUploadService
    {
        Task<CloudBlockBlob> Upload(SelectedImportDataViewModel viewModel);
    }
}