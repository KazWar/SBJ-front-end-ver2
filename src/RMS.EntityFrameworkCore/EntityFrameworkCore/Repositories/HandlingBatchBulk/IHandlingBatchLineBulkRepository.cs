using Abp.Domain.Repositories;
using RMS.SBJ.HandlingBatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.HandlingBatchBulk
{
    public interface IHandlingBatchLineBulkRepository : IRepository<HandlingBatchLine, long>
    {
        Task<List<HandlingBatchLine>> BulkInsert(List<HandlingBatchLine> handlingBatchLines);
        Task<List<HandlingBatchLine>> BulkUpdate(List<HandlingBatchLine> handlingBatchLines);
    }
}
