using System.Threading.Tasks;

namespace NetMongoDb.TenantIdentification
{
    /// <summary>
    /// Tenant Identification/Resolution interface
    /// </summary>
    public interface ITenantIdentificationStrategy
    {
        Task<string> GetTenantIdentifierAsync();
    }
}
