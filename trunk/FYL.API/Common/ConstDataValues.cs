using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYL.API.Models
{
    /// <summary>
    /// 常量数据
    /// </summary>
    public class ConstDataValues
    {
        /// <summary>
        /// 首页大图
        /// </summary>
        public static readonly int HomeBanners = 10721;

        /// <summary>
        /// 首页推荐商品
        /// </summary>
        public static readonly int HomeRecommondProducts = 10722;//10702;

        /// <summary>
        /// 商品详情页客服推荐id
        /// </summary>
        public static readonly int ProductCustomerServices = 793;

        /// <summary>
        /// 第三方登录 平台来源
        /// </summary>
        public static readonly string OpenSource = "WxApp";

        /// <summary>
        /// 商品详情页相信信息底部服务信息
        /// </summary>
        public static readonly string ProductDetailBottomService = "<img src=\"\" style=\"width: 100%;\">";

        /// <summary>
        /// 订单微信支付完成通知地址
        /// </summary>
        public static readonly string WxOrderPayNotifyUrl = "";
    }
}