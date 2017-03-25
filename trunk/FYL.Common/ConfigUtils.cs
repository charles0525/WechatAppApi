using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace FYL.Common
{
    public class ConfigUtils
    {
        /// <summary>
        /// 根据节点名获取节点值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyName">节点名</param>
        /// <param name="defaultValue">默认值(可以不填)</param>
        /// <returns></returns>
        private static T GetValue<T>(string keyName, T defaultValue = default(T))
        {
            try
            {
                object value = ConfigurationManager.AppSettings[keyName];
                if (value != null)
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
            }
            catch
            {

            }

            return defaultValue;
        }

        public static string WxAppId
        {
            get { return GetValue<string>("WxAppId"); }
        }

        public static string WxSecret
        {
            get { return GetValue<string>("WxSecret"); }
        }

        public static string WxPayMchId
        {
            get { return GetValue<string>("WxPayMchId"); }
        }

        public static string WxPaySecret
        {
            get { return GetValue<string>("wxPaySecret"); }
        }
    }
}
