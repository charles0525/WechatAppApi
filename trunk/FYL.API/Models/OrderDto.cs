using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYL.API.Models
{
    public class OrderDto
    {
        public string orderNo { get; set; }
        public string title { get; set; }
        /// <summary>
        /// 支付平台：1 ： 货到付款，2：网银支付，3：支付宝 4：微信支付 5：银联支付
        /// </summary>
        public int payFrom { get; set; }
        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string payName { get; set; }
        /// <summary>
        /// 0：待付款,1:已付款
        /// </summary>
        public int payStatus { get; set; }
        /// <summary>
        /// 邮费
        /// </summary>
        public double fare { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public double totalFee { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string statusName { get; set; }
        /// <summary>
        /// 商品总数
        /// </summary>
        public int productAmount { get; set; }
        public ShopProductItemDto[] TradeItems { get; set; }
    }
}