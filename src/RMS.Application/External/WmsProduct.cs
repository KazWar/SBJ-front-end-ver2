using System.Threading.Tasks;
using WMSProductService;

namespace RMS.External
{
    public class WmsProduct : IWmsProduct
    {
        private static readonly ProductServiceClient.EndpointConfiguration BasicHttpBinding_IProductService;

        public WmsProduct()
        {
            //...
        }

        public async Task<GetStockInfoResult> GetStockInfo(int warehouseId, string userId, string password)
        {
            var wms = new ProductServiceClient(BasicHttpBinding_IProductService);
            var stockInfo = await wms.GetStockInfoAsync(GetAuthenticationInfo(warehouseId, userId, password), false);

            return stockInfo;
        }

        private AuthenticationInfo GetAuthenticationInfo(int warehouseId, string userId, string password)
        {
            var authenticationInfo = new AuthenticationInfo
            {
                WarehouseId = warehouseId,
                UserId = userId,
                Password = password
            };

            return authenticationInfo;
        }
    }
}
