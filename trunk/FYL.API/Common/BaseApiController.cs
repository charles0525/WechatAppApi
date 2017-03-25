using System.Web.Http;
using FYL.BLL.Authorization;
using FYL.Entity.Custom.WxApp;

namespace FYL.API.Common
{
    public class BaseApiController : ApiController
    {
        private readonly UserAuthorization _auth = new UserAuthorization();

        public WxAppUserIdentity CurrentUser
        {
            get
            {
                return _auth.CurrentWxUser;
            }
        }
    }
}
