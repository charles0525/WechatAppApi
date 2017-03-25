using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYL.DAL.Order;
using SHLServiceClient.Entity;
using SHLServiceClient;
using SHLServiceClient.Entity.Trades;

namespace FYL.BLL.Order
{
    public class OrderBll
    {
        private readonly OrderDal _dal = new OrderDal();

        public Trade ConfirmOrder(int userId, int payFrom, string items, int couponId, DeliverAddress addr,
           string msg, string source, string traceReferer, string traceCode, out string errorMsg)
        {
            var result = _dal.ConfirmOrder(userId, payFrom, items, couponId, addr, msg, source, traceReferer, traceCode, out errorMsg);
            return result;
        }

        public Trade GetOrder(string orderNo)
        {
            var result = _dal.GetOrder(orderNo);
            return result;
        }

        public IEnumerable<Trade> GetOrderListByUserId(int userId, int orderType, int pageIndex, int pageSize, out string errMsg)
        {
            var result = _dal.GetOrderListByUserId(userId, orderType, pageIndex, pageSize, out errMsg);
            return result;
        }

        public TradeCountInfo GetOrderCountInfo(int userId, out string errMsg)
        {
            var result = _dal.GetOrderCountInfo(userId, out errMsg);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <param name="oprationType">1:取消订单</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool UpdateOrderStatus(int userId, string orderNo, int oprationType, out string errMsg)
        {
            var result = _dal.UpdateOrderStatus(userId, orderNo, oprationType, out errMsg);
            return result;
        }

        public void OrderPaySuccess(string orderNo, double totalFee, string buyerEmail, string sellerEmail, out string errMsg)
        {
            _dal.OrderPaySuccess(orderNo, totalFee, buyerEmail, sellerEmail, out errMsg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pay">1:货到付款，2：网银支付，3：支付宝 4：微信支付 5：银联支付</param>
        /// <returns></returns>
        public string GetPayName(int pay)
        {
            switch (pay)
            {
                case 2:
                    return "网银支付";
                case 3:
                    return "支付宝";
                case 4:
                    return "微信支付";
                case 5:
                    return "银联支付";
                default:
                    return "货到付款";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pay">1:货到付款，2：网银支付，3：支付宝 4：微信支付 5：银联支付</param>
        /// <returns></returns>
        public string GetOrderStatusName(Trade order)
        {
            if (order.Status == 1 && order.IsConfirmOrder != 5)
            {
                return "待付款";
            }
            else if (order.Status == 3)
            {
                return "待发货";
            }
            //else if (order.Status == 5)
            //{
            //    return "已发货";
            //}
            else if (order.Status == 6)
            {
                return "已退货";
            }
            else if (order.Status == 7)
            {
                return "交易完成";
            }
            else if (order.Status == 8)
            {
                return "交易关闭";
            }
            else if (order.IsConfirmOrder == 2)
            {
                return "处理中";
            }
            else if (order.IsConfirmOrder == 3)
            {
                return "已发货";
            }
            else if (order.IsConfirmOrder == 4)
            {
                return "已成功";
            }
            else if (order.IsConfirmOrder == 5)
            {
                return "已关闭";
            }
            else
                return "处理中";
        }

        public int GetOrderPayStatus(Trade order)
        {
            int[] pays = { 2, 3, 4, 5 };//支付方式，排除货到付款
            int payStatus = -1;
            if (pays.Contains(order.PayFrom))
            {
                if (order.Status != 1)
                {
                    payStatus = 1;
                }
                else if (order.IsConfirmOrder != 5)//排除交易关闭
                {
                    payStatus = 0;
                }
            }
            return payStatus;
        }
    }
}
