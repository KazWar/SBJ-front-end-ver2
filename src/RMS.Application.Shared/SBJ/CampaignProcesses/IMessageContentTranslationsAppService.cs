using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.CampaignProcesses.Dtos;
using RMS.Dto;
using System.Collections.Generic;

namespace RMS.SBJ.CampaignProcesses
{
    public interface IMessageContentTranslationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetMessageContentTranslationForViewDto>> GetAll(GetAllMessageContentTranslationsInput input);

        Task<GetMessageContentTranslationForViewDto> GetMessageContentTranslationForView(long id);

		Task<GetMessageContentTranslationForEditOutput> GetMessageContentTranslationForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditMessageContentTranslationDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetMessageContentTranslationsToExcel(GetAllMessageContentTranslationsForExcelInput input);

        Task<List<GetMessageContentTranslationForViewDto>> GetAllMessageContentTranslations();
    }
}