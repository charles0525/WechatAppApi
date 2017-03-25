using FYL.DAL.User;
using SHLServiceClient;
using SHLServiceClient.Entity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYL.Common;

namespace FYL.BLL.User
{
    public class UserBll
    {
        private readonly UserDal _dal = new UserDal();

        public SHLServiceClient.Entity.Users.User GetUserByUserId(int userId)
        {
            var result = Clients.UserService.GetUserByUserId(userId);
            return result?.Body;
        }

        public SHLServiceClient.Entity.Users.User GetUserByOpenId(string openId, string openSource)
        {
            var result = _dal.GetUserByOpenId(openId, openSource);
            return result;
        }

        public SHLServiceClient.Entity.Users.User GetUserByToken(string token, string deviceKey)
        {
            int userId = GetUserIdByToken(token, deviceKey);
            var result = GetUserByUserId(userId);

            return result;
        }

        public UserConnect GetUserConnectByToken(string token, string deviceKey)
        {
            var result = _dal.GetUserConnectByToken(token, deviceKey);
            return result;
        }

        public SHLServiceClient.Entity.Users.User UpdateUserConnect(UserConnectUpdateRequest user, out string errmsg)
        {
            var result = _dal.UpdateUserConnect(user, out errmsg);
            return result;
        }

        public UserAddress GetUserAddressByUserId(int userId)
        {
            var result = _dal.GetUserAddressByUserId(userId);
            return result;
        }

        public int GetUserId(string openId, string openSource = "WxApp")
        {
            var user = GetUserByOpenId(openId, openSource);
            if (user == null)
            {
                return 0;
            }
            return user.UserId;
        }

        public int GetUserIdByToken(string token, string deviceKey)
        {
            var bl = CheckUserTokenValid(token, deviceKey);
            if (!bl)
            {
                return 0;
            }

            var userConnect = GetUserConnectByToken(token, deviceKey);
            var userId = GetUserId(userConnect?.OpenId);
            return userId;
        }

        public int GetUserIdByToken(string token, string deviceKey,out string openId)
        {
            var bl = CheckUserTokenValid(token, deviceKey);
            openId = "";
            if (!bl)
            {
                return 0;
            }

            var userConnect = GetUserConnectByToken(token, deviceKey);
            openId = userConnect?.OpenId;
            var userId = GetUserId(userConnect?.OpenId);
            return userId;
        }

        public bool UpdateUserAddressByUserId(UserAddressUpdateRequest address, out string errMsg)
        {
            var result = _dal.UpdateUserAddressByUserId(address, out errMsg);
            return result;
        }

        public IEnumerable<UserAddressProvince> GetProvince()
        {
            var result = _dal.GetProvince();
            return result;
        }

        public string UpdateUserToken(string openId, string deviceKey, out string errMsg)
        {
            var request = new UserTokenRequest()
            {
                OpenId = openId,
                Token = GenerateToken(),
                ExpireTime = DateTime.Now.AddDays(1),
                DeviceKey = deviceKey
            };
            errMsg = "";
            _dal.UpdateUserToken(request, out errMsg);
            return request.Token;
        }

        public bool CheckUserTokenValid(string token, string deviceKey)
        {
            return _dal.CheckUserTokenValid(new UserTokenValidRequest()
            {
                Token = token,
                DeviceKey = deviceKey
            });
        }

        public string GenerateToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
