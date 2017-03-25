using FYL.API.Models;
using FYL.BLL.Product;
using FYL.BLL.Order;
using FYL.BLL.User;
using FYL.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using FYL.Common;
using System.Text.RegularExpressions;
using FYL.Pay.Wx;
using System.Web;
using System.IO;
using FYL.API.Common;
using FYL.API.Filter;

namespace FYL.API.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly ProductBll _bllProduct = new ProductBll();
        private readonly OrderBll _bllOrder = new OrderBll();
        private readonly UserBll _bllUser = new UserBll();

        #region 外部接口

        /// <summary>
        /// 获取购物车商品
        /// @2017.02.14 by charles.he
        /// </summary>
        /// <param name="ids">商品id集合(格式[商品id:skuId:数量;] 1011:75:1;1011:75:1)</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetShopProducts(string ids)
        {
            if (string.IsNullOrEmpty(ids?.Trim()))
            {
                return ResponseHelper.Fail(EnumApiStatusCode.Error, message: "参数为空!");
            }

            var data = GetProducts(ids);
            return ResponseHelper.OK(EnumApiStatusCode.Success, data: data);
        }

        /// <summary>
        /// 获取下单页面商品信息
        /// @2017.02.14 by charles.he
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetConfirmOrderProducts(string ids)
        {
            if (string.IsNullOrEmpty(ids?.Trim()))
            {
                return ResponseHelper.Fail(EnumApiStatusCode.Error, message: "参数为空!");
            }
            var rspItem = new ConfirmOrderProductDto();
            var proData = GetProducts(ids);
            rspItem.dataItems = proData;
            if (rspItem.dataItems != null && rspItem.dataItems.Count > 0)
            {
                rspItem.totalNum = rspItem.dataItems.Sum(x => x.buyNum);
                rspItem.totalMoney = rspItem.dataItems.Sum(x => x.price * x.buyNum);
            }
            return ResponseHelper.OK(EnumApiStatusCode.Success, data: rspItem);
        }

        /// <summary>
        /// 创建订单
        /// @2017.02.15 by charles.he
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [IdentityValid]
        public HttpResponseMessage CreateOrder([FromBody]OrderPostReq req)
        {
            var userId = this.CurrentUser.UserId;
            string openId = _bllUser.GetUserConnectByToken(req.token, req.deviceKey)?.OpenId;
            if (!Regex.IsMatch(req.items, @"^(\d+:\d+:\d+;?)+$"))
            {
                return ResponseHelper.OK(EnumApiStatusCode.Fail, message: "下单商品信息错误");
            }
            var userAdd = _bllUser.GetUserAddressByUserId(userId);
            if (userAdd == null)
            {
                return ResponseHelper.OK(EnumApiStatusCode.Fail, message: "请完善收货信息");
            }
            string msg = DataHelper.instance.CheckUserAddress(userAdd.RealName, userAdd.Mobile, userAdd.Province, userAdd.City, userAdd.Addr);
            if (!string.IsNullOrEmpty(msg))
            {
                return ResponseHelper.OK(EnumApiStatusCode.Fail, message: msg);
            }

            var orderAdd = new SHLServiceClient.Entity.DeliverAddress()
            {
                Mobile = userAdd.Mobile,
                RealName = userAdd.RealName,
                CityId = userAdd.City,
                AddressDetail = userAdd.Addr
            };
            string errMsg;
            var tmp = _bllOrder.ConfirmOrder(userId, req.payFrom, req.items, -1, orderAdd,
                req.remark, "wxApp", req.traceReferer, req.traceCode, out errMsg);
            if (!string.IsNullOrEmpty(errMsg?.Trim()))
            {
                return ResponseHelper.OK(EnumApiStatusCode.Fail, message: errMsg);
            }

            object result = null;
            //if (order.payFrom == EnumPayWay.Weixinzhifu.GetHashCode())
            //{
            //    try
            //    {
            //        LogHelper.Info("微信预支付开始...");
            //        string totalMoney = ((int)(tmp.TotalFee + tmp.Fare - tmp.DiscountFee)).ToString();
            //        result = WeiXinPay(tmp.OrderNo.ToString(), totalMoney, ConfigUtils.WxAppId, ConfigUtils.wxPaySecret, ConfigUtils.WxPayMchId, ConstDataValues.WxOrderPayNotifyUrl, openId);
            //        LogHelper.Info("微信预支付成功...");
            //    }
            //    catch (Exception ex)
            //    {
            //        LogHelper.Error("微信预支付失败", ex);
            //        return ResponseHelper.OK(EnumApiStatusCode.OrderPayError, message: ex.Message);
            //    }
            //}
            //else
            //{
            //    result = new OrderDto()
            //    {
            //        orderNo = tmp.OrderNo.ToString(),
            //        title = tmp.Title,
            //    };
            //}
            result = new //OrderDto()
            {
                orderNo = tmp.OrderNo.ToString(),
                title = tmp.Title,
            };

            return ResponseHelper.OK(EnumApiStatusCode.Success, data: result);
        }

        /// <summary>
        /// 未支付订单支付
        /// @2017.02.15 by charles.he
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [IdentityValid]
        public HttpResponseMessage OrderPay([FromBody]OrderPayReq request)
        {
            string openId = _bllUser.GetUserConnectByToken(request.token, request.deviceKey)?.OpenId;
            var userId = this.CurrentUser.UserId;
            if (string.IsNullOrEmpty(request.orderNo?.Trim()))
            {
                return ResponseHelper.OK(EnumApiStatusCode.Fail, message: "参数错误");
            }
            var order = _bllOrder.GetOrder(request.orderNo);
            if (order == null)
            {
                return ResponseHelper.OK(EnumApiStatusCode.Fail, message: "订单未找到");
            }

            object result = null;
            if (order.PayFrom == EnumPayWay.Weixinzhifu.GetHashCode())
            {
                try
                {
                    LogHelper.Info("微信支付开始...");
                    string totalMoney = ((int)(order.TotalFee + order.Fare - order.DiscountFee)).ToString();
                    var payConfig = new WxOrderPay()
                    {
                        AppId = ConfigUtils.WxAppId,
                        PartnerKey = ConfigUtils.WxPayMchId,
                        PaySecret = ConfigUtils.WxPaySecret,
                        orderNo = order.OrderNo.ToString(),
                        tradeAmount = totalMoney,
                        openId = openId,
                        body = "性之助商城购物",
                        notifyUrl = ConstDataValues.WxOrderPayNotifyUrl
                    };

                    result = new WeiXinPayClient().WxAppPay(payConfig);
                    LogHelper.Info("微信支付成功...");
                }
                catch (Exception ex)
                {
                    LogHelper.Error("微信支付失败", ex);
                    return ResponseHelper.OK(EnumApiStatusCode.OrderPayError, message: ex.Message);
                }
            }

            return ResponseHelper.OK(EnumApiStatusCode.Success, data: result);
        }

        /// <summary>
        /// 微信支付通知地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage OrderPayNotifyUrl()
        {
            OrderNotifyUrlReq request = GetOrderNotifyUrlXml();

            LogHelper.Info("调用支付通知地址...");
            string strRlt = @"<xml><return_code><![CDATA[{SUCCESS}]]></return_code><return_msg><![CDATA[{Msg}]]></return_msg></xml>";
            string errmsg = string.Empty;
            if (request?.return_code == "SUCCESS")
            {
                var blSign = CheckSign(request);

                if (request.result_code == "SUCCESS")
                {
                    OrderUpdateStatusSuccess(new OrderPayUpdateRequest()
                    { orderNo = request.out_trade_no, buyerEmail = "", sellerEmail = "" }, out errmsg);
                    if (!string.IsNullOrEmpty(errmsg))
                    {
                        LogHelper.Error($"/order/OrderPayNotifyUrl，订单：{request.out_trade_no} 支付完成状态更新异常：{errmsg}");
                    }
                }
                else
                {
                    strRlt = strRlt.Replace("{SUCCESS}", "FAIL").Replace("{Msg}", $"{request.err_code_des }；错误代码{request.err_code }");
                }
                strRlt = strRlt.Replace("{SUCCESS}", "SUCCESS").Replace("{Msg}", "OK");
                LogHelper.Info($"调用微信支付通知地址成功... 返回消息：{strRlt}");
            }
            else
            {
                LogHelper.Info($"调用微信支付通知地址错误...{request.return_msg}");
                strRlt = strRlt.Replace("{SUCCESS}", "FAIL").Replace("{Msg}", request.return_msg);
            }

            return new HttpResponseMessage() { Content = new StringContent(strRlt) };
        }

        /// <summary>
        /// 支付成功更新订单信息
        /// @2017.02.15 by charles.he
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage OrderPaySuccess([FromBody]OrderPayUpdateRequest request)
        {
            string errmsg;
            OrderUpdateStatusSuccess(request, out errmsg);
            if (!string.IsNullOrEmpty(errmsg))
            {
                return ResponseHelper.OK(EnumApiStatusCode.Fail, message: errmsg);
            }
            return ResponseHelper.OK(EnumApiStatusCode.Success);
        }

        #endregion

        #region 内部方法

        private List<ShopProductItemDto> GetProducts(string ids)
        {
            List<ShopProductItemDto> data = null;
            var arr = ids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (arr != null && arr.Count > 0)
            {
                data = new List<ShopProductItemDto>();
                List<string> childArr = null;
                arr.ForEach(m =>
                {
                    int itemId = 0, skuId = 0, amount = 0;
                    childArr = m.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (childArr != null && childArr.Count > 0)
                    {
                        itemId = Utils.ObjToInt(childArr[0], 0);
                    }
                    if (childArr != null && childArr.Count > 1)
                    {
                        skuId = Utils.ObjToInt(childArr[1], 0);
                    }
                    if (childArr != null && childArr.Count > 2)
                    {
                        amount = Utils.ObjToInt(childArr[2], 0);
                    }
                    if (itemId == 0)
                    {
                        return;
                    }
                    var itemList = _bllProduct.GetListByIds(new List<int>() { itemId });
                    var itemData = itemList.FirstOrDefault();
                    if (itemData == null)
                    {
                        return;
                    }
                    double decreaseValue = 0;
                    if (itemData.IsDecrease || itemData.IsLimitTimePromotion)
                    {
                        decreaseValue = _bllProduct.GetItemDecreaseAndActivity(itemList)?.FirstOrDefault()?.DecreaseValue ?? 0;
                    }

                    string skuTitle = string.Empty;
                    if (skuId > 0)
                    {
                        var skuData = _bllProduct.GetSkuList(itemId).FirstOrDefault(x => x.SkuId == skuId);
                        if (skuData != null)
                        {
                            skuTitle = skuData.Title;
                            itemData.SalePrice = skuData.Price;
                            itemData.Stock = skuData.Quantity;
                        }
                    }
                    if (amount > itemData.Stock)
                    {
                        amount = itemData.Stock;
                    }
                    data.Add(new ShopProductItemDto()
                    {
                        itemId = itemId,
                        title = itemData.Title,
                        skuTitle = skuTitle,
                        buyNum = amount,
                        imgUrl = itemData.ImgUrl,
                        price = DataHelper.instance.DoubleToDouble(Convert.ToDouble(itemData.SalePrice) - decreaseValue),
                        dataGifts = DataHelper.instance.getItemGiftItems(itemId)
                    });
                });
            }
            return data;
        }

        private void OrderUpdateStatusSuccess(OrderPayUpdateRequest request, out string errmsg)
        {
            var order = _bllOrder.GetOrder(request.orderNo);
            if (order == null)
            {
                errmsg = "订单未找到";
            }
            var totalFee = (order.TotalFee + order.Fare - order.DiscountFee);
            _bllOrder.OrderPaySuccess(request.orderNo, totalFee, request.buyerEmail, request.sellerEmail, out errmsg);
        }

        private OrderNotifyUrlReq GetOrderNotifyUrlXml()
        {
            OrderNotifyUrlReq request = new OrderNotifyUrlReq();
            if (HttpContext.Current != null)
            {
                StreamReader reader = new StreamReader(HttpContext.Current.Request.InputStream);
                String xmlData = reader.ReadToEnd();
                request = XmlUtils.Deserialize<OrderNotifyUrlReq>(xmlData);
            }
            else
            {
                LogHelper.Info("/api/order/GetOrderNotifyUrlXml=>HttpContext.Current ==null");
            }
            return request;
        }

        private bool CheckSign(OrderNotifyUrlReq request)
        {
            WeiXinPayConfig config = new WeiXinPayConfig();
            config.AppId = ConfigUtils.WxAppId;
            config.PaySecret = ConfigUtils.WxPaySecret;
            config.PartnerKey = ConfigUtils.WxPayMchId;
            WeiXinPayClient client = new WeiXinPayClient(config);
            SortedList<String, String> postParams = new SortedList<String, String>();
            postParams.Add("appid", request.appid);
            postParams.Add("mch_id", request.mch_id);
            postParams.Add("nonce_str", request.nonce_str);
            postParams.Add("body", "性之助商城购物");
            postParams.Add("out_trade_no", request.out_trade_no);
            postParams.Add("total_fee", request.total_fee.ToString());
            postParams.Add("spbill_create_ip", HttpContext.Current.Request.UserHostAddress);
            postParams.Add("notify_url", ConstDataValues.WxOrderPayNotifyUrl);
            postParams.Add("trade_type", "JSAPI");//APP
            string sign = client.CreateSign(postParams);

            return request.sign == sign;
        }

        #endregion
    }
}
