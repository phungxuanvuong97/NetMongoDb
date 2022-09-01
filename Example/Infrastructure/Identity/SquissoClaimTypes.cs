using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public static class SquissoClaimTypes
    {
        public const string ClientSecret = "urn:squisso:clientSecret";

        public const string Consent = "urn:squisso:consent";

        public const string ConsentForEmails = "urn:squisso:consent:emails";

        public const string CustomPrefix = "urn:squisso:custom";

        public const string DisplayName = "urn:squisso:name";

        public const string Hidden = "urn:squisso:hidden";

        public const string Invited = "urn:squisso:invited";

        public const string NotifoKey = "urn:squisso:notifo";

        public const string Permissions = "urn:squisso:permissions";

        public const string PictureUrl = "urn:squisso:picture";


    }
}
