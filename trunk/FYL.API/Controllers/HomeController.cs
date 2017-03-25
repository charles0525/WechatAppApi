using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FYL.Entity.DB;
using FYL.BLL.Demo;
using FYL.BLL.HotData;
using FYL.BLL.Product;
using FYL.API.Models;
using FYL.Common;
using FYL.Entity.Enum;
using FYL.API.Common;

namespace FYL.API.Controllers
{
    public class HomeController : BaseApiController
    {
        private TestBll testBll = new TestBll();
        private readonly HotDataBll _bllHotData = new HotDataBll();
        private readonly ProductBll _bllProduct = new ProductBll();

        public IList<HotDataEntity> GetHotData(int moduleId)
        {
            return testBll.GetList(moduleId);
        }

        /// <summary>
        /// 获取首页广告列表
        /// @2017.02.13 by charles.he
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetBanners()
        {
            var data = _bllHotData.GetTextAndImageData(ConstDataValues.HomeBanners).Select(x => new HomeBannerDto() { title = x.Title, imgUrl = x.Pic, linkUrl = x.Url });
            return ResponseHelper.OK(EnumApiStatusCode.Success, data: data);
        }

        /// <summary>
        /// 获取首页推荐商品
        /// @2017.02.13 by charles.he
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetProducts()
        {
            var recommendIds = _bllHotData.GetTextAndImageData(ConstDataValues.HomeRecommondProducts).Select(x => Utils.ObjToInt(x.Url, 0)).ToList();
            List<HomeProdcutItemDto> listData = new List<HomeProdcutItemDto>();
            if (recommendIds != null && recommendIds.Any())
            {
                var data = _bllProduct.GetListByIds(recommendIds);
                var decreaseValues = _bllProduct.GetItemDecreaseAndActivity(data);
                foreach (var _item in data)
                {
                    var decrease = decreaseValues.FirstOrDefault(x => x.ItemId == _item.ItemId)?.DecreaseValue ?? 0;
                    var tmp = new HomeProdcutItemDto()
                    {
                        itemId = _item.ItemId,
                        title = _item.Title,
                        price = DataHelper.instance.DoubleToDouble(_item.SalePrice - decrease),
                        imgUrl = _item.ImgUrl,
                        buys = _item.Buys
                    };
                    listData.Add(tmp);
                }
            }
            return ResponseHelper.OK(EnumApiStatusCode.Success, data: listData);
        }
    }
}
