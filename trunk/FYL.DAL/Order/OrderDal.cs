using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SHLServiceClient;
using SHLServiceClient.Entity;
using SHLServiceClient.Entity.Trades;
namespace FYL.DAL.Order
{
    public class OrderDal : BaseDal
    {
        public Trade ConfirmOrder(int userId, int payFrom, string items, int couponId, DeliverAddress addr,
            string msg, string source, string traceReferer, string traceCode, out string errorMsg)
        {
            int errorCode;
            var result = Clients.TradeService.ConfirmOrder(userId, payFrom, items, couponId, addr,
                msg, source, traceReferer, traceCode, out errorCode, out errorMsg);
            //成功
            if (errorCode == 1)
            {
                errorMsg = string.Empty;
            }

            return result;
        }

        public Trade GetOrder(string orderNo)
        {
            var result = Clients.TradeService.GetOrder(new OrderByNoRequest() { OrderNo = orderNo });
            return result?.Body;
        }

        public IEnumerable<Trade> GetOrderListByUserId(int userId, int orderType, int pageIndex, int pageSize, out string errMsg)
        {
            errMsg = string.Empty;
            var result = Clients.TradeService.GetOrderListByUserId(
                new OrderByUserIdRequest()
                { UserId = userId, OrderType = orderType, PageIndex = pageIndex, PageSize = pageSize });
            if (result.Code == StatusFail)
            {
                errMsg = result.Message;
            }
            return result?.Body;
        }

        public TradeCountInfo GetOrderCountInfo(int userId, out string errMsg)
        {
            errMsg = string.Empty;
            var result = Clients.TradeService.GetOrderCountInfo(new OrderByUserIdBaseRequest() { UserId = userId });
            if (result.Code == StatusFail)
            {
                errMsg = result.Message;
            }

            return result?.Body;
        }

        public bool UpdateOrderStatus(int userId, string orderNo, int oprationType, out string errMsg)
        {
            errMsg = string.Empty;
            var result = Clients.TradeService.UpdateOrderStatus(new OrderUpdateRequest()
            {
                UserId = userId,
                OrderNo = orderNo,
                OprationType = oprationType
            });
            if (result.Code == StatusFail)
            {
                errMsg = result.Message;
            }
            return result.Code == StatusSuccess;
        }

        public void OrderPaySuccess(string orderNo, double totalFee, string buyerEmail, string sellerEmail, out string errMsg)
        {
            errMsg = string.Empty;
            var result = Clients.TradeService.OrderPaySuccess(new OrderPaySuccessRequest()
            {
                OrderNo = orderNo,
                TotalFee = totalFee,
                BuyerEmail = buyerEmail,
                SellerEmail = sellerEmail
            });
            if (result.Code == StatusFail)
            {
                errMsg = result.Message;
            }
        }
    }
}
