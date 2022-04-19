using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace RMS.EntityFrameworkCore
{
    public static class MessagingDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<MessagingDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<MessagingDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
