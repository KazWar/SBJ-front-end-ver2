using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.PurchaseRegistrationFieldDatas.Dtos;
using RMS.Dto;


namespace RMS.SBJ.PurchaseRegistrationFieldDatas
{
    public interface IPurchaseRegistrationFieldDatasAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPurchaseRegistrationFieldDataForViewDto>> GetAll(GetAllPurchaseRegistrationFieldDatasInput input);

		Task<GetPurchaseRegistrationFieldDataForEditOutput> GetPurchaseRegistrationFieldDataForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditPurchaseRegistrationFieldDataDto input);

		Task Delete(EntityDto<long> input);

		
		Task<PagedResultDto<PurchaseRegistrationFieldDataPurchaseRegistrationFormFieldLookupTableDto>> GetAllPurchaseRegistrationFormFieldForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<PurchaseRegistrationFieldDataPurchaseRegistrationLookupTableDto>> GetAllPurchaseRegistrationForLookupTable(GetAllForLookupTableInput input);
		
    }
}