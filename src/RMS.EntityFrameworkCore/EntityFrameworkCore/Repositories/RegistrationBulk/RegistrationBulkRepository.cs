using Abp.EntityFrameworkCore;
using RMS.SBJ.Registrations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.RegistrationBulk
{
    public class RegistrationBulkRepository : RMSRepositoryBase<Registration, long>, IRegistrationBulkRepository
    {
        public RegistrationBulkRepository(IDbContextProvider<RMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        { }

        public async Task<List<Registration>> BulkInsert(List<Registration> registrations)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            await dbContext.Registrations.AddRangeAsync(registrations);
            await dbContext.SaveChangesAsync();

            return registrations;
        }

        public async Task<List<Registration>> BulkUpdate(List<Registration> registrations)
        {
            var dbContext = (RMSDbContext)base.GetDbContext();

            dbContext.Registrations.UpdateRange(registrations); //UpdateRangeAsync does not exist

            return registrations;
        }
    }
}