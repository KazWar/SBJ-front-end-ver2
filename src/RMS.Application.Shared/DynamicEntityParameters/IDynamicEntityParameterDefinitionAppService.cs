using System.Collections.Generic;

namespace RMS.DynamicEntityParameters
{
    public interface IDynamicEntityParameterDefinitionAppService
    {
        List<string> GetAllAllowedInputTypeNames();

        List<string> GetAllEntities();
    }
}
