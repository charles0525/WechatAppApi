using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYL.API.Models
{
    public class UserTokenUpdateReq
    {
        public string code { get; set; }
        public string deviceKey { get; set; }
        public string platform { get; set; }
    }
}