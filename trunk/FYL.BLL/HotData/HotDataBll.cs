using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYL.Entity.DB;
using FYL.DAL.HotData;
using System.Data;

namespace FYL.BLL.HotData
{
    public class HotDataBll
    {
        private readonly HotDataDal _dal = new HotDataDal();

        public DataTable[] GetHotDatas(int[] moduleIds)
        {
            return _dal.GetHotDatas(moduleIds);
        }

        public DataTable GetHotData(int moduleId)
        {
            return _dal.GetHotData(moduleId);
        }

        public HotTextAndImageData[] GetTextAndImageData(int moduleId)
        {
            DataTable dt = _dal.GetHotData(moduleId);
            if (dt == null)
            {
                return new HotTextAndImageData[0];
            }

            return dt.AsEnumerable().Select(item =>
            {
                HotTextAndImageData entity = new HotTextAndImageData();
                entity.Title = item.Field<string>("title");
                entity.Pic = item.Field<string>("pic");
                entity.Url = item.Field<string>("url");
                entity.Txt1 = item.Field<string>("txt1");
                entity.Txt2 = item.Field<string>("txt2");
                entity.Txt3 = item.Field<string>("txt3");

                if (dt.Columns.Contains("txt4"))
                {
                    entity.Txt4 = item.Field<string>("txt4");
                }

                if (dt.Columns.Contains("txt5"))
                {
                    entity.Txt5 = item.Field<string>("txt5");
                }

                if (dt.Columns.Contains("pic2"))
                {
                    entity.Pic2 = item.Field<string>("pic2");
                }

                if (dt.Columns.Contains("pic3"))
                {
                    entity.Pic3 = item.Field<string>("pic3");
                }

                return entity;
            }).ToArray();
        }
    }
}
