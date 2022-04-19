using Abp.EntityFrameworkCore;
using RMS.SBJ.HandlingBatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.HandlingBatchBulk
{
    public class HandlingBatchLineHistoryBulkRepository : RMSRepositoryBase<HandlingBatchLineHistory, long>, IHandlingBatchLineHistoryBulkRepository
    {
        public HandlingBatchLineHistoryBulkRepository(IDbContextProvider<RMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        { }

        public async Task<List<HandlingBatchLineHistory>> BulkInsert(List<HandlingBatchLineHistory> handlingBatchLineHistories)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            await dbContext.HandlingBatchLineHistories.AddRangeAsync(handlingBatchLineHistories);
            await dbContext.SaveChangesAsync();

            return handlingBatchLineHistories;
        }

        public async Task<List<HandlingBatchLineHistory>> BulkUpdate(List<HandlingBatchLineHistory> handlingBatchLineHistories)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            dbContext.HandlingBatchLineHistories.UpdateRange(handlingBatchLineHistories);  //UpdateRangeAsync does not exist

            return handlingBatchLineHistories;
        }
    }
}
