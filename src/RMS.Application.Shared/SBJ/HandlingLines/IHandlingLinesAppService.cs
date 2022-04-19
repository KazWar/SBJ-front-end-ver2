using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.HandlingLines.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.HandlingLines
{
    public interface IHandlingLinesAppService : IApplicationService
    {
        Task<PagedResultDto<GetHandlingLineForViewDto>> GetAll(GetAllHandlingLinesInput input);

        Task<GetHandlingLineForViewDto> GetHandlingLineForView(long id);

        Task<GetHandlingLineForEditOutput> GetHandlingLineForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHandlingLineDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHandlingLinesToExcel(GetAllHandlingLinesForExcelInput input);

        Task<List<HandlingLineCampaignTypeLookupTableDto>> GetAllCampaignTypeForTableDropdown();

        Task<PagedResultDto<HandlingLineProductHandlingLookupTableDto>> GetAllProductHandlingForLookupTable(GetAllForLookupTableInput input);

    }
}