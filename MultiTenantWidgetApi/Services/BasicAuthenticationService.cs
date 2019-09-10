using SquareWidget.BasicAuth.Core;
using System.Threading.Tasks;

namespace MultiTenantWidgetApi.Services
{
    /// <summary>
    /// Using SquareWidget.BasicAuth.Core package for authentication
    /// </summary>
    public class BasicAuthenticationService : IBasicAuthenticationService
    {
        public Task<bool> IsValidUserAsync(BasicAuthenticationOptions options, string username, string password)
        {
            return Task.FromResult(true);
        }
    }
}
