using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHLServiceClient;
using SHLServiceClient.Entity.Users;

namespace FYL.DAL.User
{
    public class UserDal : BaseDal
    {
        public SHLServiceClient.Entity.Users.User GetUserByUserId(int userId)
        {
            var result = Clients.UserService.GetUserByUserId(userId);
            return result?.Body;
        }

        public SHLServiceClient.Entity.Users.User GetUserByOpenId(string openId, string openSource)
        {
            var result = Clients.UserService.GetUserByOpenId(openId, openSource);
            return result?.Body;
        }

        public UserConnect GetUserConnectByToken(string token, string deviceKey)
        {
            var result = Clients.UserService.GetUserConnectByToken(new UserConnectByTokenRequest()
            {
                Token = token,
                DeviceKey = deviceKey
            });
            return result?.Body;
        }

        public SHLServiceClient.Entity.Users.User UpdateUserConnect(UserConnectUpdateRequest user,out string errmsg)
        {
            errmsg = string.Empty;
            var result = Clients.UserService.UpdateUserConnect(user);
            if (result.Code == StatusFail)
            {
                errmsg = result.Message;
            }

            return result?.Body;
        }

        public UserAddress GetUserAddressByUserId(int userId)
        {
            var result = Clients.UserService.GetUserAddressByUserId(new UserAddressByIdRequest() { UserId = userId });
            return result?.Body;
        }

        public bool UpdateUserAddressByUserId(UserAddressUpdateRequest user, out string errMsg)
        {
            var result = Clients.UserService.UpdateUserAddressByUserId(user);
            errMsg = "";
            if (result.Code != 0)
            {
                errMsg = result.Message;
            }

            return result.Code == 0;
        }

        public IEnumerable<UserAddressProvince> GetProvince()
        {
            var result = Clients.UserService.GetAddressProvince();
            return result?.Body;
        }

        public void UpdateUserToken(UserTokenRequest request, out string errMsg)
        {
            errMsg = "";
            var result = Clients.UserService.UpdateUserToken(request);
            if (result.Code == StatusFail)
            {
                errMsg = "Token信息更新失败";
            }
        }

        public bool CheckUserTokenValid(UserTokenValidRequest request)
        {
            var result = Clients.UserService.CheckUserTokenValid(request);
            return result.Code == StatusSuccess;
        }
    }
}
