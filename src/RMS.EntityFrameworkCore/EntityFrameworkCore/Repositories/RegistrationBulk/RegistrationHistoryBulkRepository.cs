using Abp.EntityFrameworkCore;
using RMS.SBJ.RegistrationHistory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.RegistrationBulk
{
    public class RegistrationHistoryBulkRepository : RMSRepositoryBase<RegistrationHistory, long>, IRegistrationHistoryBulkRepository
    {
        public RegistrationHistoryBulkRepository(IDbContextProvider<RMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        { }

        public async Task<List<RegistrationHistory>> BulkInsert(List<RegistrationHistory> registrationHistories)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            await dbContext.RegistrationHistories.AddRangeAsync(registrationHistories);
            await dbContext.SaveChangesAsync();

            return registrationHistories;
        }

        public async Task<List<RegistrationHistory>> BulkUpdate(List<RegistrationHistory> registrationHistories)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            dbContext.RegistrationHistories.UpdateRange(registrationHistories); //UpdateRangeAsync does not exist

            return registrationHistories;
        }
    }
}