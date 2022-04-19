using System.Threading.Tasks;
using WMSProductService;

namespace RMS.External
{
    public interface IWmsProduct
    {
        Task<GetStockInfoResult> GetStockInfo(int warehouseId, string userId, string password);
    }
}
