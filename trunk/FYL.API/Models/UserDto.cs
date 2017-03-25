using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYL.API.Models
{
    public class UserDto
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string headImgURL { get; set; }
        public string regIP { get; set; }
        public string platform { get; set; }
    }

    public class UserInfoDto : UserDto
    {
        public int gender { get; set; }
        public string regTime { get; set; }
        public string email { get; set; }
    }

    public class UserConnectReq : UserDto
    {
        public string openId { get; set; }
        public string openSource { get; set; }
    }
}