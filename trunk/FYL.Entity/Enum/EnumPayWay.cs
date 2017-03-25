using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYL.Entity.Enum
{
    /// <summary>
    /// 支付方式
    /// 1 ： 货到付款，2：网银支付，3：支付宝 4：微信支付 5：银联支付
    /// </summary>
    public enum EnumPayWay
    {
        /// <summary>
        /// 货到付款
        /// </summary>
        Huodaofukuan = 1,
        /// <summary>
        /// 网银支付
        /// </summary>
        Wangyinzhifu = 2,
        /// <summary>
        /// 支付宝
        /// </summary>
        Zhifubao = 3,
        /// <summary>
        /// 微信支付
        /// </summary>
        Weixinzhifu = 4,
        /// <summary>
        /// 银联支付
        /// </summary>
        Yinlianzhifu = 5
    }
}
