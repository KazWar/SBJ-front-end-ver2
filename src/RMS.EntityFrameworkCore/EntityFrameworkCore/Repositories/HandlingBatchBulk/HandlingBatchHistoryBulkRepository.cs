using Abp.EntityFrameworkCore;
using RMS.SBJ.HandlingBatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.HandlingBatchBulk
{
    public class HandlingBatchHistoryBulkRepository : RMSRepositoryBase<HandlingBatchHistory, long>, IHandlingBatchHistoryBulkRepository
    {
        public HandlingBatchHistoryBulkRepository(IDbContextProvider<RMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        { }

        public async Task<List<HandlingBatchHistory>> BulkInsert(List<HandlingBatchHistory> handlingBatchHistories)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            await dbContext.HandlingBatchHistories.AddRangeAsync(handlingBatchHistories);
            await dbContext.SaveChangesAsync();

            return handlingBatchHistories;
        }

        public async Task<List<HandlingBatchHistory>> BulkUpdate(List<HandlingBatchHistory> handlingBatchHistories)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            dbContext.HandlingBatchHistories.UpdateRange(handlingBatchHistories); //UpdateRangeAsync does not exist

            return handlingBatchHistories;
        }
    }
}
