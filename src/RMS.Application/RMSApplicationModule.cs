using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using RMS.Authorization;

namespace RMS
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(RMSApplicationSharedModule),
        typeof(RMSCoreModule)
        )]
    public class RMSApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RMSApplicationModule).GetAssembly());
        }
    }
}