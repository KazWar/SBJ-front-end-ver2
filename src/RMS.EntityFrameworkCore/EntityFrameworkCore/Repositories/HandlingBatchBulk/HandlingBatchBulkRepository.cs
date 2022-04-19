using Abp.EntityFrameworkCore;
using RMS.SBJ.HandlingBatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.HandlingBatchBulk
{
    public class HandlingBatchBulkRepository : RMSRepositoryBase<HandlingBatch, long>, IHandlingBatchBulkRepository
    {
        public HandlingBatchBulkRepository(IDbContextProvider<RMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        { }

        public async Task<List<HandlingBatch>> BulkInsert(List<HandlingBatch> handlingBatches)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            await dbContext.HandlingBatches.AddRangeAsync(handlingBatches);
            await dbContext.SaveChangesAsync();

            return handlingBatches;
        }

        public async Task<List<HandlingBatch>> BulkUpdate(List<HandlingBatch> handlingBatches)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            dbContext.HandlingBatches.UpdateRange(handlingBatches); //UpdateRangeAsync does not exist

            return handlingBatches;
        }
    }
}
