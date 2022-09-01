using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public static class IdentityExtensions
    {
        private const string ClientPrefix = "client_";
        private const string Type = "";

        public static string? DisplayName(this IEnumerable<Claim> user)
        {
            return user.GetClaimValue(Type);
        }
        public static string? OpenIdSubject(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(x => x.Type == OpenIdClaims.Subject)?.Value;
        }

        public static string? OpenIdClientId(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(x => x.Type == OpenIdClaims.ClientId)?.Value;
        }

        public static string? UserOrClientId(this ClaimsPrincipal principal)
        {
            return principal.OpenIdSubject() ?? principal.OpenIdClientId();
        }

        public static string? OpenIdPreferredUserName(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(x => x.Type == OpenIdClaims.PreferredUserName)?.Value;
        }

        public static string? OpenIdName(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(x => x.Type == OpenIdClaims.Name)?.Value;
        }

        public static string? OpenIdEmail(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(x => x.Type == OpenIdClaims.Email)?.Value;
        }

        public static string? GetEmail(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ??
                   principal.Claims.FirstOrDefault(x => x.Type == OpenIdClaims.Email)?.Value;
        }

        public static bool IsInClient(this ClaimsPrincipal principal, string client)
        {
            return principal.Claims.Any(x => x.Type == OpenIdClaims.ClientId && string.Equals(x.Value, client, StringComparison.OrdinalIgnoreCase));
        }

        private static string? GetClaimValue(this IEnumerable<Claim> user, string type)
        {
            return user.GetClaims(type).FirstOrDefault()?.Value;
        }

        private static IEnumerable<Claim> GetClaims(this IEnumerable<Claim> user, string request)
        {
            foreach (var claim in user)
            {
                var type = GetType(claim);

                if (type.Equals(request, StringComparison.OrdinalIgnoreCase))
                {
                    yield return claim;
                }
            }
        }

        private static ReadOnlySpan<char> GetType(Claim claim)
        {
            var type = claim.Type.AsSpan();

            if (type.StartsWith(ClientPrefix, StringComparison.OrdinalIgnoreCase))
            {
                type = type[ClientPrefix.Length..];
            }

            return type;
        }

        
    }
}
