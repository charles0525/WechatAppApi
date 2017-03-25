using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYL.DAL.Product;
using SHLServiceClient.Entity.Items;

namespace FYL.BLL.Product
{
    public class ProductBll
    {
        private readonly ProductDal _dal = new ProductDal();

        /// <summary>
        /// 获取商品集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<Item> GetListByIds(List<int> ids)
        {
            var result = _dal.GetListByIds(ids);
            return result;
        }

        /// <summary>
        /// 获取商品SKU
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public IEnumerable<ItemSku> GetSkuList(int itemId)
        {
            if (itemId <= 0)
            {
                return null;
            }

            var result = _dal.GetSkuList(itemId);
            return result;
        }

        /// <summary>
        /// 获取商品属性名称-值集合
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public IEnumerable<ItemPropertyPair> GetItemPropertyPairs(string properties)
        {
            if (string.IsNullOrEmpty(properties))
            {
                return null;
            }

            var result = _dal.GetItemPropertyPairs(properties);
            return result;
        }

        /// <summary>
        /// 获取商品赠品
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public IEnumerable<ItemGifts> GetItemGiftsByItemId(int itemId)
        {
            if (itemId <= 0)
            {
                return null;
            }

            var result = _dal.GetItemGiftsByItemId(itemId);
            return result;
        }

        /// <summary>
        /// 获取商品评价信息
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="level">0:全部,1:好评,2:中评,3:差评</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ItemEvaluation GetItemEvaluations(int itemId, int level, int pageIndex, int pageSize)
        {
            var result = _dal.GetItemEvaluations(itemId, level, pageIndex, pageSize);
            return result;
        }

        public IEnumerable<ItemImg> GetItemImgsByItemId(int itemId)
        {
            if (itemId <= 0)
            {
                return null;
            }

            var result = _dal.GetItemImgsByItemId(itemId);
            return result;
        }

        /// <summary>
        /// 获取商品拍下减优惠值
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<ItemDecreaseValue> GetItemDecrease(int[] ids)
        {
            var result = _dal.GetItemDecrease(ids);
            return result;
        }

        /// <summary>
        /// 获取商品限时抢购优惠值
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<ItemDecreaseValue> GetItemActivityDecreaseValue(int[] ids)
        {
            var result = _dal.GetItemActivityDecreaseValue(ids);
            return result;
        }

        /// <summary>
        /// 根据商品 获取 价格
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public IEnumerable<ItemDecreaseValue> GetItemDecreaseAndActivity(IEnumerable<Item> items)
        {
            if (items == null)
            {
                return null;
            }

            List<int> decreaseIds = new List<int>();
            List<int> activityIds = new List<int>();
            foreach (var _item in items)
            {
                if (_item.IsDecrease)
                {
                    decreaseIds.Add(_item.ItemId);
                }
                else if (_item.IsLimitTimePromotion)
                {
                    activityIds.Add(_item.ItemId);
                }
            }
            List<ItemDecreaseValue> result = new List<ItemDecreaseValue>();
            if (decreaseIds.Any())
            {
                var r = GetItemDecrease(decreaseIds.ToArray());
                if (r != null && r.Any())
                {
                    result.AddRange(r);
                }
            }
            if (activityIds.Any())
            {
                var r = GetItemActivityDecreaseValue(activityIds.ToArray());
                if (r != null && r.Any())
                {
                    foreach (var s in r)
                    {
                        var pro = items.FirstOrDefault(x => x.ItemId == s.ItemId);
                        var discountvalue = s.DecreaseValue;
                        double discountskuprice = Convert.ToDouble(pro.SalePrice) * (discountvalue / 100);
                        s.DecreaseValue = Math.Floor(Convert.ToDouble(pro.SalePrice) - discountskuprice);
                    }

                    result.AddRange(r);
                }
            }
            return result;
        }
    }
}
