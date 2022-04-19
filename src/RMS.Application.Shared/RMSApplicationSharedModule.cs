using Abp.Modules;
using Abp.Reflection.Extensions;

namespace RMS
{
    [DependsOn(typeof(RMSCoreSharedModule))]
    public class RMSApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RMSApplicationSharedModule).GetAssembly());
        }
    }
}