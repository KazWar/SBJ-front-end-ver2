using Abp.Modules;
using Abp.Reflection.Extensions;

namespace RMS
{
    public class RMSClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(RMSClientModule).GetAssembly());
        }
    }
}
