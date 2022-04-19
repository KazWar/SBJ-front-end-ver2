using System;
using System.Threading.Tasks;
using Abp.Webhooks;
using RMS.Authorization.Users;
using RMS.SBJ.CodeTypeTables;

namespace RMS.WebHooks
{
    public class AppWebhookPublisher : RMSDomainServiceBase, IAppWebhookPublisher
    {
        private readonly IWebhookPublisher _webHookPublisher;

        public AppWebhookPublisher(IWebhookPublisher webHookPublisher)
        {
            _webHookPublisher = webHookPublisher;
        }

        public async Task PublishTestWebhookAllAsync()
        {
            var separator = DateTime.Now.Millisecond;
            await _webHookPublisher.PublishAsync(AppWebHookNames.TestWebhook,
                new
                {
                    UserName = "Test Name " + separator,
                    EmailAddress = "Test Email " + separator
                }
            );
        }

        public async Task PublishTestWebhookTenantAsync(int? tenantId)
        {
            var separator = DateTime.Now.Millisecond;
            await _webHookPublisher.PublishAsync(AppWebHookNames.TestWebhook,
                new
                {
                    UserName = "Test Name " + separator,
                    EmailAddress = "Test Email " + separator
                },
                tenantId
            );
        }

        public async Task NewUserRegisteredAsync(User user)
        {
            await _webHookPublisher.PublishAsync(AppWebHookNames.NewUserRegistered,
                new
                {
                    Username = user.UserName,
                    EmailAddress = user.EmailAddress
                },
                null
            );
        }

        public async Task TestMakitaStatusChangedAsync(RegistrationStatus status)
        {
            await _webHookPublisher.PublishAsync(AppWebHookNames.TestMakitaStatusChanged,
                new
                {
                    StatusCode = status.StatusCode
                },
                null
            );
        }
    }
}
