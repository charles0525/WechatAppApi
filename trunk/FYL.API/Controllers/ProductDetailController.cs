using FYL.API.Models;
using FYL.BLL.Product;
using FYL.Entity.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using FYL.Common;
using FYL.API.Common;

namespace FYL.API.Controllers
{

    public class ProductDetailController : BaseApiController
    {
        private readonly ProductBll _bllProduct = new ProductBll();

        /// <summary>
        /// 获取商品详情
        /// @2017.02.14 by charles.he
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDetailInfo(int itemId)
        {
            var data = _bllProduct.GetListByIds(new List<int>() { itemId });
            ProductDetailDto rspData = new ProductDetailDto();
            if (data.Any())
            {
                var item = data.FirstOrDefault();
                if (item != null)
                {
                    double decreaseValue = 0;
                    if (item.IsDecrease || item.IsLimitTimePromotion)
                    {
                        decreaseValue = _bllProduct.GetItemDecreaseAndActivity(data)?.FirstOrDefault()?.DecreaseValue ?? 0;
                    }

                    rspData.itemId = item.ItemId;
                    rspData.title = item.Title;
                    rspData.price = DataHelper.instance.DoubleToDouble(item.SalePrice - decreaseValue);
                    rspData.stock = item.Stock;
                    rspData.marketPrice = DataHelper.instance.DoubleToDouble(item.CostPrice);
                    rspData.buys = item.Buys;
                    //rspData.properties = item.Properties;
                    //rspData.propertieNameValues = item.PropertieNameValues;
                    rspData.isFreePost = item.IsFreePost;
                    rspData.itemDesc = $"{RemoveHtmlTagExcludeImg(item.ItemDesc)}{ConstDataValues.ProductDetailBottomService}";//item.ItemDesc
                    rspData.dataEvaluation = DataHelper.instance.getItemEvaluations(itemId, 0, 1, 2);
                    rspData.dataSku = DataHelper.instance.getItemSkuItems(itemId, decreaseValue);
                    rspData.dataProperty = DataHelper.instance.getPropertyPairItems(item.Properties);
                    rspData.dataGifts = DataHelper.instance.getItemGiftItems(itemId);
                    rspData.imgs = DataHelper.instance.getItemImgs(itemId);
                    rspData.dataCustomerService = DataHelper.instance.getItemCustomerService(item.CId);
                }
            }

            return ResponseHelper.OK(EnumApiStatusCode.Success, data: rspData);
        }

        /// <summary>
        /// 获取商品赠品
        /// @2017.02.14 by charles.he
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetGiftsInfo(int itemId)
        {
            var rspData = DataHelper.instance.getItemGiftItems(itemId);
            return ResponseHelper.OK(EnumApiStatusCode.Success, data: rspData);
        }

        /// <summary>
        /// 获取商品评价
        /// @2017.02.14 by charles.he
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="level">0:全部,1:好评,2:中评,3:差评</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetEvaluationsInfo(int itemId, int level = 0, int pageIndex = 1, int pageSize = 8)
        {
            var data = DataHelper.instance.getItemEvaluations(itemId, level, pageIndex, pageSize);
            return ResponseHelper.OK(EnumApiStatusCode.Success, data: data);
        }

        /// <summary>
        /// 移除html标签（排除img）
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string RemoveHtmlTagExcludeImg(string html)
        {
            if (string.IsNullOrEmpty(html?.Trim()))
            {
                return html;
            }

            html = html.ToLower().Replace("<img", "&gt;img");
            var s = Utils.RemoveHtmlTag(html).Replace("&gt;img", "<img");
            return s;
        }
    }
}
