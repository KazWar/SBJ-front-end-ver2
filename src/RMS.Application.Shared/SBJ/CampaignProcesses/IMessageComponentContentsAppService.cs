using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.CampaignProcesses
{
    public interface IMessageComponentContentsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetMessageComponentContentForViewDto>> GetAll(GetAllMessageComponentContentsInput input);

        Task<GetMessageComponentContentForViewDto> GetMessageComponentContentForView(long id);

		Task<GetMessageComponentContentForEditOutput> GetMessageComponentContentForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditMessageComponentContentDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetMessageComponentContentsToExcel(GetAllMessageComponentContentsForExcelInput input);

        Task<List<GetMessageComponentContentForViewDto>> GetAllMessageComponentContents();
    }
}