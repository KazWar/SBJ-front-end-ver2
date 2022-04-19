using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using RMS.SBJ.HandlingBatch.Dtos;
using RMS.SBJ.HandlingBatch.Dtos.Premium;
using RMS.SBJ.HandlingBatch.Dtos.CashRefund;
using RMS.SBJ.HandlingBatch.Dtos.ActivationCode;
using RMS.Dto;

namespace RMS.SBJ.HandlingBatch
{
    public interface IHandlingBatchesAppService : IApplicationService
    {
        Task<PagedResultDto<GetHandlingBatchForViewDto>> GetAll(GetAllHandlingBatchesInput input);

        Task<GetInformationForNewPremiumBatchOutput> GetInformationForNewPremiumBatch(GetInformationForNewPremiumBatchInput input);

        Task<GetInformationForNewCashRefundBatchOutput> GetInformationForNewCashRefundBatch(GetInformationForNewCashRefundBatchInput input);

        Task<GetInformationForNewActivationCodeBatchOutput> GetInformationForNewActivationCodeBatch(GetInformationForNewActivationCodeBatchInput input);

        Task<GetPremiumBatchForView> GetPremiumBatchForView(GetPremiumBatchForData input);

        Task<GetCashRefundBatchForView> GetCashRefundBatchForView(long id);

        Task<GetActivationCodeBatchForView> GetActivationCodeBatchForView(long id);

        Task<GetHandlingBatchForViewDto> GetHandlingBatchForView(long id);

        Task<GetHandlingBatchForEditOutput> GetHandlingBatchForEdit(EntityDto<long> input);

        Task ProcessPremiums(int warehouseId, string orderUserId, string password);

        Task<FileDto> ProcessCashRefunds(long handlingBatchId, string sepaInitiator);

        Task<bool> FinishCashRefunds(long handlingBatchId);

        Task ProcessActivationCodes(long handlingBatchId);

        Task ScanWarehouseStatus(int warehouseId, string orderUserId, string password);

        Task ScanSendgridStatus();

        Task CreateOrEdit(CreateOrEditHandlingBatchDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHandlingBatchesToExcel(GetAllHandlingBatchesForExcelInput input);

        Task<FileDto> GetPremiumBatchToExcel(GetPremiumBatchForData input);

        Task<FileDto> GetCashRefundBatchToExcel(long id);     

        Task<FileDto> GetActivationCodeBatchToExcel(long id);

        Task<PagedResultDto<HandlingBatchCampaignTypeLookupTableDto>> GetAllCampaignTypeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HandlingBatchHandlingBatchStatusLookupTableDto>> GetAllHandlingBatchStatusForLookupTable(GetAllForLookupTableInput input);

    }
}