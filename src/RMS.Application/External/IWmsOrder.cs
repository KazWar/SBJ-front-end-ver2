using System.Threading.Tasks;
using WMSOrderService;

namespace RMS.External
{
    public interface IWmsOrder
    {
        Task<ImportOrderResult> ImportOrder(ImportOrder_Order orderToImport, int warehouseId, string userId, string password);

        Task<GetOrderStatusResult> GetOrderStatus(string orderReference, int warehouseId, string userId, string password);
    }
}
