using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYL.Entity.DB;
using FYL.DAL.Extend;

namespace FYL.DAL.Demo
{
    public class TestDal : BaseDal
    {
        public IList<HotDataEntity> GetList(int moduleId)
        {
            string strSql = @"select * from hotdata where ModuleId=@ModuleId";
            var data = this.DbMobileHot.GetList<HotDataEntity>(strSql, new { moduleId });
            return data;
        }
    }
}
