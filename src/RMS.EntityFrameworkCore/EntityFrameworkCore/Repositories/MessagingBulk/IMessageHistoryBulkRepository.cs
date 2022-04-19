using Abp.Domain.Repositories;
using RMS.SBJ.CampaignProcesses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.MessagingBulk
{
    public interface IMessageHistoryBulkRepository : IRepository<MessageHistory, long>
    {
        Task<List<MessageHistory>> BulkInsert(List<MessageHistory> messageHistories);
        Task<List<MessageHistory>> BulkUpdate(List<MessageHistory> messageHistories);
    }
}
