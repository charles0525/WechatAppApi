using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYL.Entity.DB;
using FYL.DAL.Extend;
using System.Data;
using SHLServiceClient;

namespace FYL.DAL.HotData
{
    public class HotDataDal : BaseDal
    {
        #region 公开方法

        public DataTable[] GetHotDatas(int[] moduleIds)
        {
            var data = Clients.HotService.GetHotDatas(moduleIds);
            return data;
        }

        public DataTable GetHotData(int moduleId)
        {
            var data = Clients.HotService.GetHotData(moduleId);
            return data;
        }

        #endregion
    }
}
