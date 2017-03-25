using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using FYL.Common;

namespace FYL.Pay.Wx
{
    public class Sha1Util
    {
        public static String GetNonceStr()
        {
            string i = Utils.GetRandom(10000);
            return FormsAuthentication.HashPasswordForStoringInConfigFile(i, "MD5");
        }

        public static string GetTimeStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return Convert.ToString((int)(time - startTime).TotalSeconds);
        }

        //创建签名SHA1
        public static String createSHA1Sign(SortedList<String, String> signParams)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            IEnumerator<KeyValuePair<string, string>> enumerator = signParams.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (i > 0)
                {
                    sb.Append("&");
                }
                String k = enumerator.Current.Key;
                String v = enumerator.Current.Value;
                sb.Append(k + "=" + v);

                i++;
            }
            return FormsAuthentication.HashPasswordForStoringInConfigFile(sb.ToString(), "SHA1").ToLower();
        }
    }
}
