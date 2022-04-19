using System.Threading.Tasks;
using Abp.Webhooks;

namespace RMS.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
