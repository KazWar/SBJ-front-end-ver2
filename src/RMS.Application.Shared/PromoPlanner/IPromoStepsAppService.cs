using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.PromoPlanner.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.PromoPlanner
{
    public interface IPromoStepsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPromoStepForViewDto>> GetAll(GetAllPromoStepsInput input);

        Task<IReadOnlyList<CustomPromoStepForView>> GetAllAsReadOnlyList(GetAllPromoStepsInput input);

        Task<GetPromoStepForViewDto> GetPromoStepForView(int id);

		Task<GetPromoStepForEditOutput> GetPromoStepForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPromoStepDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPromoStepsToExcel(GetAllPromoStepsForExcelInput input);

		
    }
}