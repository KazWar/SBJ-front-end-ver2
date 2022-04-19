using Abp.Modules;
using Abp.Reflection.Extensions;

namespace RMS
{
    public class RMSCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RMSCoreSharedModule).GetAssembly());
        }
    }
}