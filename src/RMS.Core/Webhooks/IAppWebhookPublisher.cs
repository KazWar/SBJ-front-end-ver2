using System.Threading.Tasks;
using RMS.Authorization.Users;
using RMS.SBJ.CodeTypeTables;

namespace RMS.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhookAllAsync();
        Task PublishTestWebhookTenantAsync(int? tenantId);
        Task NewUserRegisteredAsync(User user);
        Task TestMakitaStatusChangedAsync(RegistrationStatus status);
    }
}
