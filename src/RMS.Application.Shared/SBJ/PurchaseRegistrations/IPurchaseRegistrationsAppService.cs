using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.PurchaseRegistrations.Dtos;
using RMS.Dto;


namespace RMS.SBJ.PurchaseRegistrations
{
    public interface IPurchaseRegistrationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPurchaseRegistrationForViewDto>> GetAll(GetAllPurchaseRegistrationsInput input);

        Task<GetPurchaseRegistrationForViewDto> GetPurchaseRegistrationForView(long id);

		Task<GetPurchaseRegistrationForEditOutput> GetPurchaseRegistrationForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditPurchaseRegistrationDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetPurchaseRegistrationsToExcel(GetAllPurchaseRegistrationsForExcelInput input);

		
		Task<PagedResultDto<PurchaseRegistrationRegistrationLookupTableDto>> GetAllRegistrationForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<PurchaseRegistrationProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<PurchaseRegistrationHandlingLineLookupTableDto>> GetAllHandlingLineForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<PurchaseRegistrationRetailerLocationLookupTableDto>> GetAllRetailerLocationForLookupTable(GetAllForLookupTableInput input);
		
    }
}