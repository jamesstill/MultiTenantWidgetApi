using MultiTenantWidgetApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MultiTenantWidgetApi.Services
{
public class CatalogService
{
    private readonly TenantSettings _tenantSettings;

    public CatalogService(TenantSettings tenantSettings)
    {
        _tenantSettings = tenantSettings ??
            throw new ArgumentNullException(nameof(tenantSettings));
    }

    public async Task<string> GetConnectionString()
    {
        // fetch the tenant from the catalog
        var tenant = await GetTenantFromCatalog(_tenantSettings.TenantId);
        if (tenant == null)
        {
            throw new Exception("Tenant not found in catalog!");
        }

        var builder = new SqlConnectionStringBuilder(_tenantSettings.DefaultConnectionString)
        {
            DataSource = tenant.DatabaseServerName,
            InitialCatalog = tenant.DatabaseName
        };

        return builder.ConnectionString;
    }

    /// <summary>
    /// Stub to simulate a call out to an API, Azure Table Storage, DB, 
    /// or the location of the catalog you implement in your project.
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    private async Task<Tenant> GetTenantFromCatalog(string tenantId)
    {
        var list = new List<Tenant>()
        {
            new Tenant {
                TenantId = "tenant1",
                DatabaseServerName = "tcp:tenant1.database.windows.net,1433",
                DatabaseName = "tenant1db"
            },
            new Tenant {
                TenantId = "tenant2",
                DatabaseServerName = "tcp:tenant2.database.windows.net,1433",
                DatabaseName = "tenant2db"
            },
            new Tenant {
                TenantId = "tenant3",
                DatabaseServerName = "tcp:tenant3.database.windows.net,1433",
                DatabaseName = "tenant3db"
            }
        };

        await Task.CompletedTask;
        return list
            .Where(t => t.TenantId == tenantId)
            .SingleOrDefault() ?? new Tenant();
    }
}
}
