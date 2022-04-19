using Abp.Domain.Repositories;
using RMS.SBJ.RegistrationHistory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.RegistrationBulk
{
    public interface IRegistrationHistoryBulkRepository : IRepository<RegistrationHistory, long>
    {
        Task<List<RegistrationHistory>> BulkInsert(List<RegistrationHistory> registrationHistories);
        Task<List<RegistrationHistory>> BulkUpdate(List<RegistrationHistory> registrationHistories);
    }
}