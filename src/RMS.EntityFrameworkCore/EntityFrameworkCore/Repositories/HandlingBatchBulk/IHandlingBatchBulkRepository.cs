using Abp.Domain.Repositories;
using RMS.SBJ.HandlingBatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.HandlingBatchBulk
{
    public interface IHandlingBatchBulkRepository : IRepository<HandlingBatch, long>
    {
        Task<List<HandlingBatch>> BulkInsert(List<HandlingBatch> handlingBatches);
        Task<List<HandlingBatch>> BulkUpdate(List<HandlingBatch> handlingBatches);
    }
}
