using Abp.EntityFrameworkCore;
using RMS.SBJ.CampaignProcesses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.MessagingBulk
{
    public class MessageHistoryBulkRepository : RMSRepositoryBase<MessageHistory, long>, IMessageHistoryBulkRepository
    {
        public MessageHistoryBulkRepository(IDbContextProvider<RMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        { }

        public async Task<List<MessageHistory>> BulkInsert(List<MessageHistory> messageHistories)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            await dbContext.MessageHistories.AddRangeAsync(messageHistories);
            await dbContext.SaveChangesAsync();

            return messageHistories;
        }

        public async Task<List<MessageHistory>> BulkUpdate(List<MessageHistory> messageHistories)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            dbContext.MessageHistories.UpdateRange(messageHistories); //UpdateRangeAsync does not exist

            return messageHistories;
        }
    }
}
