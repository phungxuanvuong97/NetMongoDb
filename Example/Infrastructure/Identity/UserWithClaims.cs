using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public sealed class UserWithClaims : IUser
    {
        public IdentityUser Identity { get; }

        public string Id
        {
            get => Identity.Id;
        }

        public string Email
        {
            get => Identity.Email;
        }

        public bool IsLocked
        {
            get => Identity.LockoutEnd > DateTime.UtcNow;
        }

        public IReadOnlyList<Claim> Claims { get; }

        object IUser.Identity => Identity;

        public UserWithClaims(IdentityUser user, IReadOnlyList<Claim> claims)
        {
            Identity = user;

            Claims = claims;
        }
    }
}
