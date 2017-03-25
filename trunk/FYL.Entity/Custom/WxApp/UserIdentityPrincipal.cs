using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FYL.Entity.Custom.WxApp
{
    public class UserIdentityPrincipal : IPrincipal
    {
        public UserIdentityPrincipal(WxAppUserIdentity identity)
        {
            Identity = identity;
        }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}
