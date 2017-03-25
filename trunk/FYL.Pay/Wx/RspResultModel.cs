using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYL.Pay.Wx
{
    public class BaseRsp
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
    }

    public class SessionkeyRsp : BaseRsp
    {
        public string openid { get; set; }
        public string session_key { get; set; }
    }

    public class WxOrderPayResult
    {
        public string return_code { get; set; }
        public string return_msg { get; set; }
        public string result_code { get; set; }
        public string trade_state { get; set; }
        public string trade_state_desc { get; set; }
    }
}
