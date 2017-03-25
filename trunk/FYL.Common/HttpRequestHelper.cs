using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FYL.Common
{
    /// <summary>
    /// HTTP请求帮助类
    /// </summary>
    public class HttpRequestHelper
    {
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postParam">请求参数</param>
        /// <param name="contentType"></param>
        /// <returns>流信息</returns>
        public static string DoPost(string url, string postParam, string contentType = "application/x-www-form-urlencoded")
        {
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            var param = Encoding.UTF8.GetBytes(postParam);
            HttpWebRequest req = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                req = WebRequest.Create(url) as HttpWebRequest;
                req.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                req = WebRequest.Create(url) as HttpWebRequest;
            }
            req.Method = "POST";
            req.ContentType = contentType;
            req.ContentLength = param.Length;
            req.KeepAlive = false;
            req.ProtocolVersion = HttpVersion.Version10;
            using (var reqstream = req.GetRequestStream())
            {
                reqstream.Write(param, 0, param.Length);
            }
            using (var response = (HttpWebResponse)req.GetResponse())
            {
                var stream = response.GetResponseStream();
                if (stream == null)
                    return string.Empty;
                var reader = new StreamReader(stream);
                var data = reader.ReadToEnd();
                return data;
            }
        }

        /// <summary>
        /// 处理GET请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="getParam">请求参数</param>
        /// <returns>流信息</returns>
        public static string DoGet(string url, string getParam = "")
        {
            var req = WebRequest.Create(url + "?" + getParam) as HttpWebRequest;
            req.Method = "GET";
            using (var response = (HttpWebResponse)req.GetResponse())
            {
                var stream = response.GetResponseStream();
                if (stream == null)
                    return string.Empty;
                var reader = new StreamReader(stream);
                var data = reader.ReadToEnd();
                return data;
            }
        }

        /// <summary>
        /// 处理GET请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="getParam">请求参数</param>
        /// <returns>流信息</returns>
        public static string DoGet(string url, string getParam = "", string encode = "UTF-8")
        {
            var req = WebRequest.Create(url + "?" + getParam) as HttpWebRequest;
            req.Method = "GET";
            using (var response = (HttpWebResponse)req.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream = null;
                    byte[] buffer = new byte[response.ContentLength];
                    using (stream = response.GetResponseStream())
                    {
                        stream.Read(buffer, 0, buffer.Length);
                    }
                    return Encoding.GetEncoding(encode).GetString(buffer);
                }
                return "";
                //var stream = response.GetResponseStream();
                //if (stream == null)
                //    return string.Empty;
                //var reader = new StreamReader(stream);
                //var data = reader.ReadToEnd();
                //return data;
            }
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // 总是接受  
            return true;
        }
    }

    public class HttpContentType
    {
        public const string application_json = "application/json";
        public const string text_plain = "text/plain";
    }
}
