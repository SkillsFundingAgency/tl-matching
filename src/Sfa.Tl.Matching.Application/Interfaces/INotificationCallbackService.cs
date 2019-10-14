using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface INotificationCallbackService
    {
        Task<int> HandleNotificationCallbackAsync(string payload);
    }
}