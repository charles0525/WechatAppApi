using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FYL.Entity.Custom.WxApp
{
    public class WxAppUserIdentity : IIdentity
    {
        public WxAppUserIdentity(UserIdentityBase user = null)
        {
            if (user != null)
            {
                IsAuthenticated = true;
                Name = user.Name;
                UserId = user.UserId;
            }
        }

        public string AuthenticationType { get { return "WxAppAuthentication"; } }

        public bool IsAuthenticated { get; private set; }

        public string Name { get; private set; }

        public int UserId { get; private set; }
    }
}
