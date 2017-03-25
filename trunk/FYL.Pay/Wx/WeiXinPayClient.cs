using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FYL.Common;
using System.Web.Security;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FYL.Pay.Wx
{
    public class WeiXinPayClient
    {
        const string AccessTokenCacheKey = "WeiXin_AccessToken";
        //private static MemcachedHelper memcached = new MemcachedHelper();

        private WeiXinPayConfig config;
        private string accessToken;

        public WeiXinPayClient(WeiXinPayConfig config)
        {
            this.config = config;
        }

        public WeiXinPayClient()
        {
            this.config = new WeiXinPayConfig();
        }

        public String GetToken()
        {
            accessToken = "";// memcached.Get(AccessTokenCacheKey) as string;
            if (string.IsNullOrEmpty(accessToken))
            {
                string requestUrl = WeiXinPayConfig.TokenUrl + "?grant_type=client_credential&appid=" + config.AppId + "&secret=" + config.AppSecret;

                string result = HttpRequestHelper.DoGet(requestUrl, "", WeiXinPayConfig.Charset);
                if (!string.IsNullOrEmpty(result))
                {

                    JObject obj = JObject.Parse(result);
                    JToken token;
                    if (obj.TryGetValue("access_token", out token))
                    {
                        accessToken = token.ToString().Replace("\"", "");
                        //memcached.Set(AccessTokenCacheKey, accessToken, DateTime.Now.AddMinutes(100));
                    }
                }
            }
            else
            {
                accessToken = accessToken.Replace("\"", "");
            }
            return accessToken;
        }

        // 获取package带参数的签名包
        public String GenPackage(SortedList<string, string> packageParams)
        {
            String sign = CreateSign(packageParams);
            StringBuilder sb = new StringBuilder();
            IEnumerator<KeyValuePair<string, string>> enumerator = packageParams.GetEnumerator();
            while (enumerator.MoveNext())
            {
                String k = enumerator.Current.Key;
                String v = enumerator.Current.Value;
                sb.Append(k + "=" + UrlEncode(v) + "&");
            }
            return sb.Append("sign=" + sign).ToString();
        }

        public String CreateXml(SortedList<string, string> packageParams)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");

            IEnumerator<KeyValuePair<string, string>> enumerator = packageParams.GetEnumerator();
            while (enumerator.MoveNext())
            {
                String k = enumerator.Current.Key;
                String v = enumerator.Current.Value;
                sb.AppendFormat("<{0}>{1}</{0}>", k, v);
            }

            sb.Append("</xml>");
            return sb.ToString();
        }


        public String UrlEncode(String src)
        {
            string s = null;
            StringBuilder sb = new StringBuilder();
            foreach (char c in src)
            {
                s = Utils.UrlEncode(c.ToString());
                if (s.Length > 1)
                {
                    sb.Append(s.Replace("+", "%20").ToUpper());
                }
                else
                {
                    sb.Append(c);
                }

            }
            return sb.ToString();
        }


        public String CreateSign(SortedList<string, string> packageParams)
        {
            StringBuilder sb = new StringBuilder();
            //IEnumerator<KeyValuePair<string, string>> enumerator = packageParams.GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    String k = enumerator.Current.Key;
            //    String v = enumerator.Current.Value;
            //    if (null != v && k != "sign" && k != "key")
            //    {
            //        sb.Append(k + "=" + v + "&");
            //    }
            //}

            var vDic = (from objDic in packageParams orderby objDic.Key ascending select objDic);
            foreach (var kv in vDic)
            {
                String k = kv.Key;
                String v = kv.Value;
                if (!string.IsNullOrEmpty(v) && k != "sign" && k != "key")
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + config.PaySecret);
            return FormsAuthentication.HashPasswordForStoringInConfigFile(sb.ToString(), "MD5").ToUpper();
        }

        // 提交预支付
        public String SendPrepay(SortedList<string, string> packageParams)
        {
            String prepayid = string.Empty;

            String postData = "{";

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in packageParams)
            {
                if (kv.Key != "appkey")
                {
                    if (postData.Length > 1)
                        postData += ",";
                    postData += "\"" + kv.Key + "\":\"" + kv.Value + "\"";
                }
            }

            postData += "}";

            // 设置链接参数
            String requestUrl = WeiXinPayConfig.GateUrl + "?access_token=" + this.accessToken;

            string result = HttpRequestHelper.DoPost(requestUrl, postData, WeiXinPayConfig.Charset);
            if (!string.IsNullOrEmpty(result))
            {
                JObject obj = JObject.Parse(result);
                string errcode = obj["errcode"].ToString();
                if (errcode == "0")
                {
                    prepayid = obj["prepayid"].ToString();
                }
                else if (errcode == "40001")
                {
                    //memcached.Delete(AccessTokenCacheKey);
                }
            }
            return prepayid;
        }

        public String Unifiedorder(SortedList<string, string> postParams)
        {
            String prepayid = string.Empty;

            String postData = CreateXml(postParams);

            // 设置链接参数
            String requestUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

            string content = HttpRequestHelper.DoPost(requestUrl, postData, WeiXinPayConfig.Charset);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);
            XmlNode node = doc.DocumentElement.SelectSingleNode("./prepay_id");
            if (node == null)
            {
                throw new Exception(content);
            }

            if (string.IsNullOrEmpty(node.InnerText))
            {
                //Utils.WriteLog(content);
            }

            return node.InnerText;

        }

        #region 微信小程序

        /// <summary>
        /// code 换取 session_key,openid
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public SessionkeyRsp GetSessionkeyOpenIdByCode(string code)
        {
            var url = WeiXinPayConfig.SessionkeyAndOpenidUrl
                .Replace("{appId}", config.AppId).Replace("{secret}", config.AppSecret).Replace("{code}", code);
            var result = HttpRequestHelper.DoGet(url, "");
            var rsp = JsonConvert.DeserializeObject<SessionkeyRsp>(result);
            return rsp;
        }

        /// <summary>
        /// 查询订单支付状态
        /// </summary>
        /// <param name="postParams"></param>
        /// <returns></returns>
        public WxOrderPayResult QueryOrderPayResult(SortedList<string, string> postParams)
        {
            String postData = CreateXml(postParams);
            string content = HttpRequestHelper.DoPost("https://api.mch.weixin.qq.com/pay/orderquery", postData, WeiXinPayConfig.Charset);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            var result = XmlUtils.Deserialize<WxOrderPayResult>(content);
            return result;
        }

        public string WxAppPay(WxOrderPay pay)
        {
            this.config = pay;
            string noncestr = Sha1Util.GetNonceStr();

            SortedList<String, String> postParams = new SortedList<String, String>();
            postParams.Add("appid", pay.AppId);
            postParams.Add("mch_id", pay.PartnerKey);
            postParams.Add("nonce_str", noncestr);
            postParams.Add("body", pay.body);
            postParams.Add("out_trade_no", pay.orderNo);
            postParams.Add("total_fee", (double.Parse(pay.tradeAmount) * 100).ToString());//元转分
            postParams.Add("spbill_create_ip", System.Web.HttpContext.Current.Request.UserHostAddress);
            postParams.Add("notify_url", pay.notifyUrl);
            postParams.Add("trade_type", "JSAPI");//APP
            postParams.Add("openid", pay.openId);
            string sign = this.CreateSign(postParams);

            postParams.Add("sign", sign);
            string prepayid = this.Unifiedorder(postParams);

            SortedList<String, String> outParams = new SortedList<String, String>();
            outParams.Add("appId", config.AppId);
            outParams.Add("timeStamp", Sha1Util.GetTimeStamp(DateTime.Now));
            outParams.Add("nonceStr", noncestr);
            //outParams.Add("partnerid", config.PartnerKey);
            outParams.Add("package", $"prepay_id={prepayid}");
            //outParams.Add("prepayid", prepayid);
            outParams.Add("signType", "MD5");

            //生成签名
            sign = this.CreateSign(outParams);
            outParams.Add("sign", sign);
            outParams.Add("retcode", "0");
            outParams.Add("retmsg", "成功");

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(outParams);
            return result;
        }

        #endregion
    }
}
