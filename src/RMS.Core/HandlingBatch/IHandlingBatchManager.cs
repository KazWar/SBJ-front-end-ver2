using Abp.Domain.Services;
using System.Threading.Tasks;

namespace RMS.HandlingBatchManager
{
    public interface IHandlingBatchManager : IDomainService
    {
        Task ProcessPremiums(int batchSize);
    }
}
