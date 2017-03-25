using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYL.API.Models
{
    public class OrderPostReq : RequestBase
    {
        /// <summary>
        /// 1 ： 货到付款，2：网银支付，3：支付宝 4：微信支付 5：银联支付
        /// </summary>
        public int payFrom { get; set; }
        /// <summary>
        /// 商品信息[商品id:skuId:数量;] 1011:75:1;1011:75:1
        /// </summary>
        public string items { get; set; }
        /// <summary>
        /// 优惠券id
        /// </summary>
        //public int couponId { get; set; }

        public string remark { get; set; }
        /// <summary>
        /// 支付来源
        /// </summary>
        //public string source { get; set; }
        /// <summary>
        /// 页面来源
        /// </summary>
        public string traceReferer { get; set; }
        /// <summary>
        /// 跟踪代码
        /// </summary>
        public string traceCode { get; set; }
    }
}