using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RMS.Configuration;
using RMS.Web;

namespace RMS.EntityFrameworkCore
{
    public class MessagingDbContextFactory : IDesignTimeDbContextFactory<MessagingDbContext>
    {
        public MessagingDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MessagingDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            MessagingDbContextConfigurer.Configure(builder, configuration.GetConnectionString(RMSConsts.MessagingConnectionStringName));

            return new MessagingDbContext(builder.Options);
        }
    }
}
