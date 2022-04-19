using RMS.SBJ.HandlingBatch.Models;
using System.Threading.Tasks;

namespace RMS.SBJ.HandlingBatch
{
    public interface IHandlingCashRefundBatchesBackgroundJob
    {
        Task<string> ExecuteSepaAsync(HandlingBatchJobParameters parameters);

        Task<bool> ExecutePaidAsync(HandlingBatchJobParameters parameters);
    }
}
