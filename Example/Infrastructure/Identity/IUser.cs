using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public interface IUser
    {
        bool IsLocked { get; }

        string Id { get; }

        string Email { get; }

        object Identity { get; }

        IReadOnlyList<Claim> Claims { get; }
    }
}
