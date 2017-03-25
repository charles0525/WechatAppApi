using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHLServiceClient;
using SHLServiceClient.Entity.Items;

namespace FYL.DAL.Product
{
    public class ProductDal : BaseDal
    {
        public IEnumerable<Item> GetListByIds(List<int> ids)
        {
            IEnumerable<Item> result = null;
            var data = Clients.ItemService.GetItemById(new ItemByIdRequest() { Ids = ids.ToArray() });
            if (data != null)
            {
                result = data.Body;
            }
            return result;
        }

        public IEnumerable<ItemSku> GetSkuList(int itemId)
        {
            IEnumerable<ItemSku> result = null;
            var data = Clients.ItemService.GetSkuListByItemId(new ItemSkuByItemIdRequest() { ItemId = itemId });
            if (data != null)
            {
                result = data.Body;
            }
            return result;
        }

        public IEnumerable<ItemPropertyPair> GetItemPropertyPairs(string properties)
        {
            IEnumerable<ItemPropertyPair> result = null;
            var data = Clients.ItemService.GetItemPropertyPairs(new ItemPropertyRequest() { Properties = properties });
            if (data != null)
            {
                result = data.Body;
            }
            return result;
        }

        public IEnumerable<ItemGifts> GetItemGiftsByItemId(int itemId)
        {
            IEnumerable<ItemGifts> result = null;
            var data = Clients.ItemService.GetItemGiftsListByItemId(new ItemGiftsByIdRequest() { ItemId = itemId });
            if (data != null)
            {
                result = data.Body;
            }
            return result;
        }

        public ItemEvaluation GetItemEvaluations(int itemId, int level, int pageIndex, int pageSize)
        {
            ItemEvaluation result = null;
            var data = Clients.ItemService.GetItemEvaluationsByItemId(new ItemEvaluationByIdRequest()
            { ItemId = itemId, Level = level, PageIndex = pageIndex, PageSize = pageSize });
            if (data != null && data.Body != null)
            {
                result = data.Body;
            }
            return result;
        }

        public IEnumerable<ItemImg> GetItemImgsByItemId(int itemId)
        {
            IEnumerable<ItemImg> result = null;
            var data = Clients.ItemService.GetItemImg(new ItemImgByIdRequest() { ItemId = itemId });
            if (data != null)
            {
                result = data.Body;
            }
            return result;
        }

        /// <summary>
        /// 获取商品拍下减优惠值
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<ItemDecreaseValue> GetItemDecrease(int[] ids)
        {
            IEnumerable<ItemDecreaseValue> result = null;
            var request = new ItemByIdRequest() { Ids = ids };
            var data = Clients.ItemService.GetItemDecrease(request);
            if (data != null)
            {
                result = data.Body;
            }
            return result;
        }

        /// <summary>
        /// 获取商品限时抢购优惠值
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<ItemDecreaseValue> GetItemActivityDecreaseValue(int[] ids)
        {
            IEnumerable<ItemDecreaseValue> result = null;
            var data = Clients.ItemService.GetItemActivityDecreaseValue(new ItemByIdRequest() { Ids = ids });
            if (data != null)
            {
                result = data.Body;
            }
            return result;
        }
    }
}
