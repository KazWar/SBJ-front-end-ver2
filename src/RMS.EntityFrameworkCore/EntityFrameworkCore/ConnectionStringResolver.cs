using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RMS.Configuration;
using System;

namespace RMS.EntityFrameworkCore
{
    public class ConnectionStringResolver : DefaultConnectionStringResolver, IDbPerTenantConnectionStringResolver
    {
        public IAbpSession AbpSession { get; set; }
        private readonly IConfigurationRoot _appConfiguration;
        private readonly ITenantCache _tenantCache;
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public ConnectionStringResolver(IAbpStartupConfiguration configuration, IWebHostEnvironment hostingEnvironment, 
            ITenantCache tenantCache, ICurrentUnitOfWorkProvider currentUnitOfWorkProvider) : base(configuration)
        {
            _appConfiguration = AppConfigurations.Get(hostingEnvironment.ContentRootPath, hostingEnvironment.EnvironmentName);
            _tenantCache = tenantCache;
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        public override string GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            if (args.MultiTenancySide == MultiTenancySides.Host)
            {
                return GetNameOrConnectionString(new DbPerTenantConnectionStringResolveArgs(null, args));
            }

            if (args["DbContextConcreteType"] as Type == typeof(MessagingDbContext))
            {
                return _appConfiguration.GetConnectionString(RMSConsts.MessagingConnectionStringName);
            }

            return GetNameOrConnectionString(new DbPerTenantConnectionStringResolveArgs(GetCurrentTenantId(), args));
        }

        public string GetNameOrConnectionString(DbPerTenantConnectionStringResolveArgs args)
        {
            if (args.TenantId == null)
            {
                //Requested for host
                return base.GetNameOrConnectionString(args);
            }

            var tenantCacheItem = _tenantCache.Get(args.TenantId.Value);
            if (tenantCacheItem.ConnectionString.IsNullOrEmpty())
            {
                //Tenant has not dedicated database
                return base.GetNameOrConnectionString(args);
            }

            return tenantCacheItem.ConnectionString;
        }

        protected virtual int? GetCurrentTenantId()
        {
            return _currentUnitOfWorkProvider.Current != null
                ? _currentUnitOfWorkProvider.Current.GetTenantId()
                : AbpSession.TenantId;
        }
    }
}
