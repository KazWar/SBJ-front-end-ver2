using Abp.Application.Features;
using Abp.Localization;
using Abp.Webhooks;
using RMS.Features;

namespace RMS.WebHooks
{
    public class AppWebhookDefinitionProvider : WebhookDefinitionProvider
    {
        public override void SetWebhooks(IWebhookDefinitionContext context)
        {
            context.Manager.Add(new WebhookDefinition(
                name: AppWebHookNames.TestWebhook
            ));

            //Add your webhook definitions here 
            context.Manager.Add(new WebhookDefinition(
                name: AppWebHookNames.NewUserRegistered,
                displayName: L("NewUserRegisteredWebhookDefinition")
            ));

            context.Manager.Add(new WebhookDefinition(
                name: AppWebHookNames.TestMakitaStatusChanged,
                displayName: L("TestMakitaStatusChangedWebhookDefinition"),
                description: L("DescriptionTestMakitaStatusChangedWebhookDefinition"),
                featureDependency: new SimpleFeatureDependency(AppFeatures.TestCheckFeature)
                ));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, RMSConsts.LocalizationSourceName);
        }
    }
}
