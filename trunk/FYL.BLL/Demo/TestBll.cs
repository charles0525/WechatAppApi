using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYL.Entity.DB;
using FYL.DAL.Demo;

namespace FYL.BLL.Demo
{
    public class TestBll
    {
        private TestDal dal = new TestDal();

        public IList<HotDataEntity> GetList(int moduleId)
        {
            return dal.GetList(moduleId);
        }
    }
}
