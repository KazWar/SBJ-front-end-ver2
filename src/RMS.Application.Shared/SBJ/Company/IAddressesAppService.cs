using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.Company.Dtos;
using RMS.Dto;


namespace RMS.SBJ.Company
{
    public interface IAddressesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAddressForViewDto>> GetAll(GetAllAddressesInput input);

        Task<GetAddressForViewDto> GetAddressForView(long id);

		Task<GetAddressForEditOutput> GetAddressForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditAddressDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetAddressesToExcel(GetAllAddressesForExcelInput input);

		
    }
}