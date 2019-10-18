using System;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IEmailDeliveryStatusService
    {
        Task<int> HandleEmailDeliveryStatusAsync(string payload);
        Task SendEmailDeliveryStatusAsync(Guid notificationId);
    }
}