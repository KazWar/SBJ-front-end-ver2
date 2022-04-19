using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.PurchaseRegistrationFormFields.Dtos;
using RMS.Dto;


namespace RMS.SBJ.PurchaseRegistrationFormFields
{
    public interface IPurchaseRegistrationFormFieldsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPurchaseRegistrationFormFieldForViewDto>> GetAll(GetAllPurchaseRegistrationFormFieldsInput input);

        Task<GetPurchaseRegistrationFormFieldForViewDto> GetPurchaseRegistrationFormFieldForView(long id);

		Task<GetPurchaseRegistrationFormFieldForEditOutput> GetPurchaseRegistrationFormFieldForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditPurchaseRegistrationFormFieldDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetPurchaseRegistrationFormFieldsToExcel(GetAllPurchaseRegistrationFormFieldsForExcelInput input);

		
		Task<PagedResultDto<PurchaseRegistrationFormFieldFormFieldLookupTableDto>> GetAllFormFieldForLookupTable(GetAllForLookupTableInput input);
		
    }
}