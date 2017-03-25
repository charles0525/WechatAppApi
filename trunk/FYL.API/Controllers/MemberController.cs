using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FYL.BLL.User;
using FYL.BLL.Order;
using FYL.BLL.Product;
using FYL.API.Models;
using FYL.Common;
using FYL.Pay.Wx;
using FYL.API.Common;
using FYL.API.Filter;

namespace FYL.API.Controllers
{
    public class MemberController : BaseApiController
    {
        private readonly UserBll _bllUser = new UserBll();
        private readonly OrderBll _bllOrder = new OrderBll();
        private readonly ProductBll _bllProduct = new ProductBll();

        /// <summary>
        /// 获取收货地址
        /// @2017.02.14 by charles.he
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpGet]
        [IdentityValid]
        public HttpResponseMessage GetUserAddress([FromUri]RequestBase request)
        {
            int userId = this.CurrentUser.UserId;
            var data = _bllUser.GetUserAddressByUserId(userId);
            var result = new UserAddressDto();
            if (data != null)
            {
                result.address = data.Addr;
                result.city = data.City;
                result.mobile = data.Mobile;
                result.postCode = data.PostCode;
                result.province = data.Province;
                result.realName = data.RealName;
                result.remark = data.Remark;
                result.tel = data.Tel;
            }

            return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Success, data: result);
        }

        /// <summary>
        /// 更新用户收货地址信息
        /// @2017.02.14 by charles.he
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [IdentityValid]
        public HttpResponseMessage UpdateUserAddress([FromBody]UpdateUserAddressReq model)
        {
            int userId = this.CurrentUser.UserId;
            string tipMsg = DataHelper.instance.CheckUserAddress(model.realName, model.mobile, model.province, model.city, model.address);
            if (!string.IsNullOrEmpty(tipMsg))
            {
                return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Fail, message: tipMsg);
            }

            var request = new SHLServiceClient.Entity.Users.UserAddressUpdateRequest()
            {
                Addr = model.address,
                City = model.city,
                Mobile = model.mobile,
                PostCode = model.postCode,
                Province = model.province,
                RealName = model.realName,
                Remark = model.remark,
                Tel = model.tel,
                UserId = userId,
                AddrType = 1
            };
            string errMsg;
            var result = _bllUser.UpdateUserAddressByUserId(request, out errMsg);
            if (!result)
            {
                return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Fail, message: errMsg);
            }
            return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Success, message: "操作成功");
        }

        /// <summary>
        /// 获取用户信息
        /// @2017.02.14 by charles.he
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserInfo([FromUri]RequestBase request)
        {
            var user = _bllUser.GetUserByToken(request.token, request.deviceKey);
            var result = new UserInfoDto()
            {
                userId = user.UserId,
                userName = user.UserName,
                headImgURL = user.HeadImgURL,
                gender = user.Gender,
                regTime = user.RegTime.ToString("yyyy-MM-dd HH:mm:ss"),
                email = user.Email
            };
            return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Success, data: result);
        }

        /// <summary>
        /// 更新授权登录用户信息(停用)
        /// @2017.02.14 by charles.he
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateUserConnect([FromBody]UserConnectReq user)
        {
            if (string.IsNullOrEmpty(user.openId?.Trim()))
            {
                return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Fail, "openId为空");
            }
            var request = new SHLServiceClient.Entity.Users.UserConnectUpdateRequest()
            {
                OpenId = user.openId,
                OpenSource = ConstDataValues.OpenSource,
                UserIP = user.regIP,
                NickName = user.userName,
                RegSource = "wx_App",
                RegType = "wxApp",
                HeadImgURL = user.headImgURL,
                Platform = user.platform//使用终端 1:Android ,2:Ios,3:WindowsPhone 4-PC商城
            };
            var errmsg = string.Empty;
            var result = _bllUser.UpdateUserConnect(request, out errmsg);
            if (result != null && result.UserId > 0)
            {
                return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Success, data: result);
            }
            return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Fail, message: "用户信息更新失败");
        }

        /// <summary>
        /// 获取订单数量统计
        /// @2017.02.14 by charles.he
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpGet]
        [IdentityValid]
        public HttpResponseMessage GetOrderCountInfo([FromUri]RequestBase request)
        {
            var userId = this.CurrentUser.UserId;
            string errMsg;
            var data = _bllOrder.GetOrderCountInfo(userId, out errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Fail, message: errMsg);
            }
            return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Success, data: data);
        }

        /// <summary>
        /// 获取订单列表
        /// @2017.02.15 by charles.he
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="orderType">订单筛选:0全部,1:待付款,2:待发货,3:已发货</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [IdentityValid]
        public HttpResponseMessage GetOrderList(string token, string deviceKey, int orderType = 0, int pageIndex = 1, int pageSize = 8)
        {
            int userId = this.CurrentUser.UserId;
            string errMsg;
            var data = _bllOrder.GetOrderListByUserId(userId, orderType, pageIndex, pageSize, out errMsg);
            if (!string.IsNullOrEmpty(errMsg?.Trim()))
            {
                return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Fail, message: errMsg);
            }
            IEnumerable<OrderDto> listData = null;
            if (data != null && data.Any())
            {
                var productIds = new List<int>();
                data?.ToList().ForEach(x =>
                {
                    var ids = x?.TradeItems.Select(m => m.ItemId).ToList();
                    if (ids != null && ids.Any())
                    {
                        productIds.AddRange(ids);
                    }
                });
                var listProduct = _bllProduct.GetListByIds(productIds);
                listData = data.Select(x => new OrderDto()
                {
                    orderNo = x.OrderNo.ToString(),
                    title = x.Title,
                    payFrom = x.PayFrom,
                    payName = _bllOrder.GetPayName(x.PayFrom),
                    payStatus = _bllOrder.GetOrderPayStatus(x),
                    fare = DataHelper.instance.DoubleToDouble(x.Fare),
                    totalFee = DataHelper.instance.DoubleToDouble(Convert.ToInt64(x.TotalFee)),
                    statusName = _bllOrder.GetOrderStatusName(x),
                    productAmount = x.TradeItems?.Sum(s => s.Amount) ?? 0,
                    TradeItems = x.TradeItems?.Select(m => new ShopProductItemDto()
                    {
                        title = m.Title,
                        itemId = m.ItemId,
                        price = DataHelper.instance.DoubleToDouble(m.Price),
                        buyNum = m.Amount,
                        skuTitle = m.SkuPropName,
                        imgUrl = listProduct.FirstOrDefault(y => y.ItemId == m.ItemId)?.ImgUrl,
                        dataGifts = m.GiftData?.Select(n => new ItemGift()
                        {
                            title = n.Title,
                            giftId = n.ItemId,
                            img = n.ImgUrl,
                            num = n.Amount
                        })
                    }).ToArray()
                });
            }

            return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Success, data: listData);
        }

        /// <summary>
        /// 取消订单
        /// @2017.02.15 by charles.he
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpPost]
        [IdentityValid]
        public HttpResponseMessage CancelOrder([FromBody]UpdateOrderReq req)
        {
            int userId = this.CurrentUser.UserId;
            string errMsg;
            _bllOrder.UpdateOrderStatus(userId, req.orderNo.ToString(), 1, out errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Fail, message: errMsg);
            }
            return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Success, message: "操作成功");
        }

        /// <summary>
        /// 根据code获取token
        /// @2017.02.16 by charles.he
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetTokenByCode([FromBody] UserTokenUpdateReq req)
        {
            string wxappId = ConfigUtils.WxAppId;
            string wxSecret = ConfigUtils.WxSecret;
            var wxConfig = new WeiXinPayConfig()
            {
                AppId = wxappId,
                AppSecret = wxSecret
            };
            var wxClient = new WeiXinPayClient(wxConfig);
            var wxInfo = wxClient.GetSessionkeyOpenIdByCode(req.code);
            if (wxInfo.errcode > 0)
            {
                return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Fail, message: wxInfo.errmsg);
            }
            string errmsg;
            var token = _bllUser.UpdateUserToken(wxInfo.openid, req.deviceKey, out errmsg);
            if (!string.IsNullOrEmpty(errmsg?.Trim()))
            {
                LogHelper.Warning($"openid:{wxInfo.openid} 生成token信息失败");
                return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Fail, message: $"{errmsg}；" + $"openid:{wxInfo.openid} 生成token信息失败");
            }

            var request = new SHLServiceClient.Entity.Users.UserConnectUpdateRequest()
            {
                OpenId = wxInfo.openid,
                OpenSource = ConstDataValues.OpenSource,
                UserIP = Utils.GetIP(),
                NickName = $"wx_{Guid.NewGuid().ToString("N")}",
                RegSource = "wx_App",
                RegType = "wxApp",
                HeadImgURL = "",
                Platform = req.platform//使用终端 1:Android ,2:Ios,3:WindowsPhone 4-PC商城
            };
            var result = _bllUser.UpdateUserConnect(request, out errmsg);
            if (result == null)
            {
                LogHelper.Warning($"openid:{wxInfo.openid} 生成用户信息失败,错误消息：{errmsg}");
                return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Fail, message: $"{errmsg}；" + $"openid:{wxInfo.openid} 生成用户信息失败");
            }

            return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Success, data: token);
        }

        /// <summary>
        /// 校验token
        /// @2017.02.16 by charles.he
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CheckUserToken([FromBody]RequestBase request)
        {
            var result = _bllUser.CheckUserTokenValid(request.token, request.deviceKey);
            if (!result)
            {
                return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Fail, message: "Token校验失败或过期");
            }
            return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Success);
        }

        /// <summary>
        /// 获取用户信息
        /// @2017.02.16 by charles.he
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [IdentityValid]
        public HttpResponseMessage GetUserInfoByToken([FromUri]RequestBase request)
        {
            var user = _bllUser.GetUserByUserId(this.CurrentUser.UserId);
            return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Success, data: user);
        }
    }
}
