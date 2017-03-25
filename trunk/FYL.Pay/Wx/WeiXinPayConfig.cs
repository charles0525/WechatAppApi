using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYL.Pay.Wx
{
    public class WeiXinPayConfig
    {
        /// <summary>
        /// 获取Token网关
        /// </summary>
        public const string TokenUrl = "https://api.weixin.qq.com/cgi-bin/token";

        /// <summary>
        /// 提交预支付单网关
        /// </summary>
        public const string GateUrl = "https://api.weixin.qq.com/pay/genprepay";

        /// <summary>
        /// 验证notify支付订单网关
        /// </summary>
        public const string NotifyUrl = "https://gw.tenpay.com/gateway/simpleverifynotifyid.xml";

        public const string Charset = "UTF-8";

        /// <summary>
        /// 获取session_key 和 openid 地址
        /// </summary>
        public const string SessionkeyAndOpenidUrl = "https://api.weixin.qq.com/sns/jscode2session?appid={appId}&secret={secret}&js_code={code}&grant_type=authorization_code";

        public string AppId { get; set; }
        public string PartnerKey { get; set; }
        public string AppSecret { get; set; }
        public string PaySecret { get; set; }
    }

    public class WxOrderPay : WeiXinPayConfig
    {
        public string orderNo { get; set; }
        public string tradeAmount { get; set; }
        public string notifyUrl { get; set; }
        public string openId { get; set; }
        public string body { get; set; } 
    }
}
