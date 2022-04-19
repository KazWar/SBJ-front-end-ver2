using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.PurchaseRegistrationFields.Dtos;
using RMS.Dto;


namespace RMS.SBJ.PurchaseRegistrationFields
{
    public interface IPurchaseRegistrationFieldsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPurchaseRegistrationFieldForViewDto>> GetAll(GetAllPurchaseRegistrationFieldsInput input);

		Task<GetPurchaseRegistrationFieldForEditOutput> GetPurchaseRegistrationFieldForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditPurchaseRegistrationFieldDto input);

		Task Delete(EntityDto<long> input);

		
		Task<PagedResultDto<PurchaseRegistrationFieldFormFieldLookupTableDto>> GetAllFormFieldForLookupTable(GetAllForLookupTableInput input);
		
    }
}