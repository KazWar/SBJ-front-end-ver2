using RMS.SBJ.HandlingBatch.Models;
using System.Threading.Tasks;

namespace RMS.SBJ.HandlingBatch
{
    public interface IScanWarehouseStatusBackgroundJob
    {
        Task ExecuteAsync(HandlingBatchJobParameters parameters);
    }
}
