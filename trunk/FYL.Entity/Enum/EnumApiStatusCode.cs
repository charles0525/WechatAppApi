using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYL.Entity.Enum
{
    public enum EnumApiStatusCode
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 请求失败
        /// </summary>
        Fail = 0,
        /// <summary>
        /// 服务器错误
        /// </summary>
        Error = -1,

        /// <summary>
        /// 订单预支付失败
        /// </summary>
        OrderPayError = -2,
        /// <summary>
        /// 获取用户信息校验失败
        /// </summary>
        GetUserFail = -3
    }
}
