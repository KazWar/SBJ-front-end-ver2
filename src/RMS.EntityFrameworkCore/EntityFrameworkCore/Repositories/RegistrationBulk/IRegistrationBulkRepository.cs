using Abp.Domain.Repositories;
using RMS.SBJ.Registrations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.EntityFrameworkCore.Repositories.RegistrationBulk
{
    public interface IRegistrationBulkRepository : IRepository<Registration, long>
    {
        Task<List<Registration>> BulkInsert(List<Registration> registrations);
        Task<List<Registration>> BulkUpdate(List<Registration> registrations);
    }
}