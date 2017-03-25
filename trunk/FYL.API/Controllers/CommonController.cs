using FYL.BLL.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FYL.API.Models;
using FYL.API.Common;

namespace FYL.API.Controllers
{
    public class CommonController : BaseApiController
    {
        private readonly UserBll _bllUser = new UserBll();

        /// <summary>
        /// 获取省市
        /// @2017.02.15 by charles.he
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetProvince()
        {
            var data = _bllUser.GetProvince();
            var result = data.Select(x => new UserAddressProvinceDto()
            {
                id = x.Id,
                name = x.Name,
                citys = x.Citys?.Select(m => new UserAddressCity()
                {
                    id = m.Id,
                    name = m.Name,
                    //parentId = m.ParentId
                })
            });
            return ResponseHelper.OK(Entity.Enum.EnumApiStatusCode.Success, data: result);
        }
    }
}
