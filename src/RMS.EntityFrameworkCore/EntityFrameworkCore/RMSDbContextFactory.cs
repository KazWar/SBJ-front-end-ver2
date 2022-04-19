using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RMS.Configuration;
using RMS.Web;

namespace RMS.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class RMSDbContextFactory : IDesignTimeDbContextFactory<RMSDbContext>
    {
        public RMSDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<RMSDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            RMSDbContextConfigurer.Configure(builder, configuration.GetConnectionString(RMSConsts.ConnectionStringName));

            return new RMSDbContext(builder.Options);
        }
    }
}