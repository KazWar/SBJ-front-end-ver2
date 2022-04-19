using RMS.HandlingBatchManager;
using System.Threading.Tasks;

namespace RMS.Web.Controllers
{
    public class JobController
    {
        private readonly IHandlingBatchManager _handlingBatchManager;

        public JobController(IHandlingBatchManager handlingBatchManager)
        {
            _handlingBatchManager = handlingBatchManager;
        }

        public async Task ProcessPremiums(int batchSize)
        {
            await _handlingBatchManager.ProcessPremiums(batchSize);
        }
    }
}
