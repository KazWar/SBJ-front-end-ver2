using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace RMS.Startup
{
    [DependsOn(typeof(RMSCoreModule))]
    public class RMSGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RMSGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}