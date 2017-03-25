using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYL.Framework.DataProvider;

namespace FYL.DAL
{
    public class BaseDal
    {
        protected readonly PmhMallDataProvider DbMall = PmhMallDataProvider.instance;
        protected readonly PmhMobileDataProvider DbMobile = PmhMobileDataProvider.instance;
        protected readonly PmhMobileHotDataProvider DbMobileHot = PmhMobileHotDataProvider.instance;

        /// <summary>
        /// 针对wcf服务端接口返回状态值
        /// </summary>
        public const int StatusSuccess = 0;
        public const int StatusFail = -1;

        public BaseDal()
        {

        }
    }
}
