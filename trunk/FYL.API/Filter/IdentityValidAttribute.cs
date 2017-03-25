using FYL.BLL.User;
using FYL.Common;
using FYL.Entity;
using FYL.Entity.Custom;
using FYL.Entity.Custom.WxApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FYL.API.Filter
{
    public class IdentityValidAttribute : AuthorizationFilterAttribute
    {
        private readonly UserBll _bllUser = new UserBll();

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var token = Utils.GetReqFormValue("token");
            var deviceKey = Utils.GetReqFormValue("deviceKey");
            if (string.IsNullOrEmpty(token))
            {
                throw new CustomException("token is null or invalid!");
            }
            if (string.IsNullOrEmpty(deviceKey))
            {
                throw new CustomException("deviceKey is null or invalid!");
            }
            var user = _bllUser.GetUserByToken(token, deviceKey);
            if (user == null || user.UserId <= 0)
            {
                throw new CustomException("token or deviceKey invalid,or user is null!");
            }
            var identityUser = new WxAppUserIdentity(new UserIdentityBase()
            {
                UserId = user.UserId,
                Name = user.UserName
            });
            SetPrincipal(new UserIdentityPrincipal(identityUser));

            //base.OnAuthorization(actionContext);
        }

        private void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
    }
}