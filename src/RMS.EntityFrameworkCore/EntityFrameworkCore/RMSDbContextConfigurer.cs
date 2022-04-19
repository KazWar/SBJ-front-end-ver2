using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace RMS.EntityFrameworkCore
{
    public static class RMSDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<RMSDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<RMSDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}