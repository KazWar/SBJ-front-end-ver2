using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.MakitaSerialNumber.Dtos;
using RMS.Dto;

namespace RMS.SBJ.MakitaSerialNumber
{
    public interface IMakitaSerialNumbersAppService : IApplicationService
    {
        Task<PagedResultDto<GetMakitaSerialNumberForViewDto>> GetAll(GetAllMakitaSerialNumbersInput input);

        Task<GetMakitaSerialNumberForViewDto> GetMakitaSerialNumberForView(long id);

        Task<GetMakitaSerialNumberForEditOutput> GetMakitaSerialNumberForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditMakitaSerialNumberDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetMakitaSerialNumbersToExcel(GetAllMakitaSerialNumbersForExcelInput input);

    }
}