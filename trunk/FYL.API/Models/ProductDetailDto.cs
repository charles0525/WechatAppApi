using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYL.Common;

namespace FYL.API.Models
{
    public class ProductDetailDto
    {
        public int itemId { get; set; }
        public string title { get; set; }
        public string[] imgs { get; set; }
        public double price { get; set; }
        public int stock { get; set; }
        public double marketPrice { get; set; }
        public int buys { get; set; }
        //public string properties { get; set; }
        //public string propertieNameValues { get; set; }
        public bool isFreePost { get; set; }
        public string itemDesc { get; set; }

        /// <summary>
        /// 评价
        /// </summary>
        public ItemEvaluation dataEvaluation { get; set; }
        /// <summary>
        /// Sku
        /// </summary>
        public List<ItemSku> dataSku { get; set; }
        /// <summary>
        /// 属性集合
        /// </summary>
        public List<ItemPropertyPair> dataProperty { get; set; }
        /// <summary>
        /// 赠品
        /// </summary>
        public List<ItemGift> dataGifts { get; set; }
        /// <summary>
        /// 客服信息
        /// </summary>
        public ItemCustomerServiceInfo dataCustomerService { get; set; }

        public class Helper
        {
            public static List<ItemSku> getSkuItems(IEnumerable<SHLServiceClient.Entity.Items.ItemSku> data, double decreaseValue = 0)
            {
                if (data == null || data.Count() == 0)
                {
                    return null;
                }
                return data.Select(x => new ItemSku()
                {
                    skuId = x.SkuId,
                    title = x.Title,
                    properties = x.Properties,
                    costPrice = x.CostPrice,
                    price = DataHelper.instance.DoubleToDouble(Convert.ToDouble(x.Price) - decreaseValue),
                    stock = x.Quantity
                }).ToList();
            }

            public static List<ItemPropertyPair> getPropertyPairItems(IEnumerable<SHLServiceClient.Entity.Items.ItemPropertyPair> data)
            {
                if (data == null || data.Count() == 0)
                {
                    return null;
                }
                return data.Select(x => new ItemPropertyPair()
                {
                    propertyName = x.PropertyName,
                    propertyValue = x.PropertyValue
                }).ToList();
            }

            public static List<ItemGift> getGiftItems(IEnumerable<SHLServiceClient.Entity.Items.ItemGifts> data)
            {
                if (data == null || data.Count() == 0)
                {
                    return null;
                }
                return data.Select(x => new ItemGift()
                {
                    title = x.Title,
                    giftId = x.GiftItemId,
                    img = x.ImgUrl,
                    num = x.GiftNum
                }).ToList();
            }

            public static ItemEvaluation getItemEvaluationItem(SHLServiceClient.Entity.Items.ItemEvaluation data)
            {
                var result = new ItemEvaluation();
                if (data != null)
                {
                    result.totalNum = data.TotalNum;
                    result.goodNum = data.GoodNum;
                    result.minNum = data.MinNum;
                    result.badNum = data.BadNum;
                    if (data.Data != null && data.Data.Any())
                    {
                        result.data = data.Data.Select(x => new ItemEvaluationItem()
                        {
                            reviewId = x.ReviewId,
                            userId = x.UserId,
                            nickName = x.Nick == "匿名" ? x.Nick : Utils.DealUserName(x.Nick),
                            content = x.Content,
                            createTime = x.Created.ToString("yyyy-MM-dd"),
                            starLevel = x.StarLevel,
                            reviewContent = x.ReviewContent
                        });
                    }
                }
                return result;
            }
        }
    }

    /// <summary>
    /// sku
    /// </summary>
    public class ItemSku
    {
        public int skuId { get; set; }
        public string title { get; set; }
        public string properties { get; set; }
        public string costPrice { get; set; }
        public double price { get; set; }
        public int stock { get; set; }
    }

    public class ItemEvaluation
    {
        public ItemEvaluation()
        {
        }

        /// <summary>
        /// 总数
        /// </summary>
        public int totalNum { get; set; }
        /// <summary>
        /// 好评数
        /// </summary>
        public int goodNum { get; set; }
        /// <summary>
        /// 中评数
        /// </summary>
        public int minNum { get; set; }
        /// <summary>
        /// 差评数
        /// </summary>
        public int badNum { get; set; }
        /// <summary>
        /// 列表集合
        /// </summary>
        public IEnumerable<ItemEvaluationItem> data;
    }

    /// <summary>
    /// 评价
    /// </summary>
    public class ItemEvaluationItem
    {
        public int reviewId { get; set; }
        public int userId { get; set; }
        public string nickName { get; set; }
        public string content { get; set; }
        public string createTime { get; set; }
        public int starLevel { get; set; }
        public string reviewContent { get; set; }
    }

    public class ItemPropertyPair
    {
        public string propertyName { get; set; }
        public string propertyValue { get; set; }
    }

    /// <summary>
    /// 商品详情客服信息
    /// </summary>
    public class ItemCustomerServiceInfo
    {
        public string title { get; set; }
        public string imgUrl { get; set; }
    }
}