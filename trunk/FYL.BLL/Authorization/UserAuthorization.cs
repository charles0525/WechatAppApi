using FYL.Entity.Custom.WxApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FYL.BLL.Authorization
{
    public class UserAuthorization
    {
        public WxAppUserIdentity CurrentWxUser
        {
            get
            {
                var context = HttpContext.Current;
                if (context == null || context.User == null)
                {
                    return new WxAppUserIdentity();
                }
                return context.User.Identity as WxAppUserIdentity;
            }
        }
    }
}
