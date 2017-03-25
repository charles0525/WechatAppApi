using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using FYL.Entity.Enum;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace FYL.API.Models
{
    public class ResponseHelper
    {
        /// <summary>
        ///     成功消息
        /// </summary>
        public static HttpResponseMessage OK(EnumApiStatusCode code, string message = "", object data = null)
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = Content(code, message, data) };
        }

        /// <summary>
        ///     成功消息
        /// </summary>
        public static HttpResponseMessage OK(object dto)
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = Content(dto) };
        }

        /// <summary>
        ///     逻辑错误
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public static HttpResponseMessage Fail(EnumApiStatusCode code, string message = "", object data = null)
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = Content(code, message, data) };
        }

        public static StringContent Content(EnumApiStatusCode code, string message = "", object data = null)
        {
            return new StringContent(JsonConvert.SerializeObject(new { code = code.GetHashCode(), message, data }));
        }

        private static StringContent Content(object dto)
        {
            return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8);
        }
    }
}