using RMS.SBJ.HandlingBatch.Models;
using System.Threading.Tasks;

namespace RMS.SBJ.HandlingBatch
{
    public interface IHandlingActivationCodeBatchesBackgroundJob
    {
        Task ExecuteAsync(HandlingBatchJobParameters parameters);
    }
}
