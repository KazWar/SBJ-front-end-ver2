using Abp.Domain.Repositories;
using RMS.SBJ.HandlingBatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.HandlingBatchBulk
{
    public interface IHandlingBatchHistoryBulkRepository : IRepository<HandlingBatchHistory, long>
    {
        Task<List<HandlingBatchHistory>> BulkInsert(List<HandlingBatchHistory> handlingBatchHistories);
        Task<List<HandlingBatchHistory>> BulkUpdate(List<HandlingBatchHistory> handlingBatchHistories);
    }
}
