using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYL.API.Models
{
    public class ProdcutItemBase
    {
        public int itemId { get; set; }
        public string title { get; set; }
        public double price { get; set; }
        public string imgUrl { get; set; }
    }

    public class HomeProdcutItemDto : ProdcutItemBase
    {
        public int buys { get; set; }
    }

    /// <summary>
    /// 购物车商品信息
    /// </summary>
    public class ShopProductItemDto : ProdcutItemBase
    {
        /// <summary>
        /// 购买数量
        /// </summary>
        public int buyNum { get; set; }
        public string skuTitle { get; set; }
        public IEnumerable<ItemGift> dataGifts { get; set; }
    }

    /// <summary>
    /// 下单页商品信息
    /// </summary>
    public class ConfirmOrderProductDto
    {
        public int totalNum { get; set; }
        public double totalMoney { get; set; }

        public List<ShopProductItemDto> dataItems { get; set; }
    }
}