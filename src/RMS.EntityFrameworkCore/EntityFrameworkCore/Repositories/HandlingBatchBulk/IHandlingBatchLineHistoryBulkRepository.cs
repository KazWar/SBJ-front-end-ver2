using Abp.Domain.Repositories;
using RMS.SBJ.HandlingBatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.HandlingBatchBulk
{
    public interface IHandlingBatchLineHistoryBulkRepository : IRepository<HandlingBatchLineHistory, long>
    {
        Task<List<HandlingBatchLineHistory>> BulkInsert(List<HandlingBatchLineHistory> handlingBatchLineHistories);
        Task<List<HandlingBatchLineHistory>> BulkUpdate(List<HandlingBatchLineHistory> handlingBatchLineHistories);
    }
}
