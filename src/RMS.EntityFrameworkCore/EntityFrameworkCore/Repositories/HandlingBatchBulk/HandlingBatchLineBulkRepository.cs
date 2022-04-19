using Abp.EntityFrameworkCore;
using RMS.SBJ.HandlingBatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.HandlingBatchBulk
{
    public class HandlingBatchLineBulkRepository : RMSRepositoryBase<HandlingBatchLine, long>, IHandlingBatchLineBulkRepository
    {
        public HandlingBatchLineBulkRepository(IDbContextProvider<RMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        { }

        public async Task<List<HandlingBatchLine>> BulkInsert(List<HandlingBatchLine> handlingBatchLines)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            await dbContext.HandlingBatchLines.AddRangeAsync(handlingBatchLines);
            await dbContext.SaveChangesAsync();

            return handlingBatchLines;
        }

        public async Task<List<HandlingBatchLine>> BulkUpdate(List<HandlingBatchLine> handlingBatchLines)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            dbContext.HandlingBatchLines.UpdateRange(handlingBatchLines); //UpdateRangeAsync does not exist

            return handlingBatchLines;
        }
    }
}
