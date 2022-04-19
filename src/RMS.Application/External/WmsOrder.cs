using System.Threading.Tasks;
using WMSOrderService;

namespace RMS.External
{
    public class WmsOrder : IWmsOrder
    {   
        private static readonly OrderServiceClient.EndpointConfiguration BasicHttpBinding_IOrderService;

        public WmsOrder()
        {
            //...
        }

        public async Task<ImportOrderResult> ImportOrder(ImportOrder_Order orderToImport, int warehouseId, string userId, string password)
        {
            var wms = new OrderServiceClient(BasicHttpBinding_IOrderService);
            var orderResult = await wms.ImportOrderAsync(GetAuthenticationInfo(warehouseId, userId, password), orderToImport, false, false);

            return orderResult;
        }

        public async Task<GetOrderStatusResult> GetOrderStatus(string orderReference, int warehouseId, string userId, string password)
        {
            var wms = new OrderServiceClient(BasicHttpBinding_IOrderService);
            var orderStatus = await wms.GetOrderStatusAsync(GetAuthenticationInfo(warehouseId, userId, password), orderReference, false);

            return orderStatus;
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
