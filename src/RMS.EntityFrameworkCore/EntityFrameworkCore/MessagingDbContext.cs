using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RMS.SBJ.Messaging;

namespace RMS.EntityFrameworkCore
{
    public class MessagingDbContext : AbpDbContext
    {
        public MessagingDbContext(DbContextOptions<MessagingDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Messages> Messages { get; set; }

    }
}
