using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetMongoDb.Storage
{
    public interface ITenantStorage<T> where T : Tenant
    {
        Task<T> GetTenantAsync(string identifier);
    }
}
