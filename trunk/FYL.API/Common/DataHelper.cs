using FYL.API.Models;
using FYL.BLL.Product;
using FYL.BLL.HotData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYL.Common;

namespace FYL.API.Models
{
    /// <summary>
    /// 主要用于数据转换
    /// </summary>
    public class DataHelper
    {
        public static readonly DataHelper instance = new DataHelper();
        private readonly ProductBll _bllProduct = new ProductBll();
        private readonly HotDataBll _bllHotData = new HotDataBll();

        /// <summary>
        /// 获取评价信息
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="level"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ItemEvaluation getItemEvaluations(int itemId, int level, int pageIndex, int pageSize)
        {
            var data = _bllProduct.GetItemEvaluations(itemId, level, pageIndex, pageSize);
            var result = new ItemEvaluation();
            if (data != null)
            {
                result = ProductDetailDto.Helper.getItemEvaluationItem(data);
            }
            return result;
        }

        public List<ItemSku> getItemSkuItems(int itemId, double decreaseValue = 0)
        {
            var data = _bllProduct.GetSkuList(itemId);
            var result = new List<ItemSku>();
            if (data != null)
            {
                result = ProductDetailDto.Helper.getSkuItems(data, decreaseValue);
            }
            return result;
        }

        public List<ItemPropertyPair> getPropertyPairItems(string properties)
        {
            var data = _bllProduct.GetItemPropertyPairs(properties);
            var result = new List<ItemPropertyPair>();
            if (data != null)
            {
                result = ProductDetailDto.Helper.getPropertyPairItems(data);
            }
            return result;
        }

        public List<ItemGift> getItemGiftItems(int itemId)
        {
            var data = _bllProduct.GetItemGiftsByItemId(itemId);
            var result = new List<ItemGift>();
            if (data != null)
            {
                result = ProductDetailDto.Helper.getGiftItems(data);
            }
            return result;
        }

        public string[] getItemImgs(int itemId)
        {
            var data = _bllProduct.GetItemImgsByItemId(itemId);
            var result = new string[] { };
            if (data != null)
            {
                result = data.Select(x => x.ImgPath).ToArray();
            }
            return result;
        }

        /// <summary>
        /// 根据产品分类获取 商品对应客服信息
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public ItemCustomerServiceInfo getItemCustomerService(int cid)
        {
            var hotDatas = _bllHotData.GetTextAndImageData(ConstDataValues.ProductCustomerServices)
                ?.Where(x => x.Url == cid.ToString())?.Select(x => new ItemCustomerServiceInfo()
                {
                    title = x.Title,
                    imgUrl = x.Pic
                })?.FirstOrDefault();
            return hotDatas;
        }

        public string CheckUserAddress(string realName, string mobile, int province, int city, string address)
        {
            string tipMsg = string.Empty;
            if (string.IsNullOrEmpty(realName?.Trim()))
            {
                tipMsg = "请输入收货人";
            }
            else if (string.IsNullOrEmpty(mobile?.Trim()))
            {
                tipMsg = "请输入手机号码";
            }
            else if (!Utils.IsMobile(mobile))
            {
                tipMsg = "手机号输入错误";
            }
            else if (province <= 0)
            {
                tipMsg = "请选择省份";
            }
            else if (city <= 0)
            {
                tipMsg = "请选择城市";
            }
            else if (string.IsNullOrEmpty(address?.Trim()))
            {
                tipMsg = "请输入详细地址";
            }
            return tipMsg;
        }

        public string DoubleToStr(object s)
        {
            return string.Format("{0:0.00}", s);
        }

        public double DoubleToDouble(object s)
        {
            return double.Parse(string.Format("{0:0.00}", s));
        }
    }
}