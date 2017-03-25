using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FYL.Framework.DataProvider;

namespace FYL.DAL.Extend
{
    /// <summary>
    /// 数据访问扩展方法
    /// by charles.he
    /// @2016.12.26
    /// </summary>
    public static class DataProviderExtend
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dataProvider"></param>
        /// <param name="strSql">查询语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static IList<T> GetList<T>(this BaseDataProvider dataProvider, string strSql, object param)
        {
            var idbCon = dataProvider.GetIDbConnection();
            var data = idbCon.Query<T>(strSql, param);
            idbCon.Close();
            return data.ToList();
        }

        /// <summary>
        /// 获取行数
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <param name="strSql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int GetCount(this BaseDataProvider dataProvider, string strSql, object param)
        {
            var idbCon = dataProvider.GetIDbConnection();
            var data = idbCon.ExecuteScalar<int>(strSql, param);
            idbCon.Close();
            return data;
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dataProvider"></param>
        /// <param name="strSql">查询语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static T GetOne<T>(this BaseDataProvider dataProvider, string strSql, object param)
        {
            var idbCon = dataProvider.GetIDbConnection();
            var data = idbCon.Query<T>(strSql, param);
            idbCon.Close();
            return data.FirstOrDefault();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <param name="strSql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int Delete(this BaseDataProvider dataProvider, string strSql, object param)
        {
            var idbCon = dataProvider.GetIDbConnection();
            var data = idbCon.Execute(strSql, param);
            idbCon.Close();
            return data;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <param name="strSql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int Update(this BaseDataProvider dataProvider, string strSql, object param)
        {
            var idbCon = dataProvider.GetIDbConnection();
            var data = idbCon.Execute(strSql, param);
            idbCon.Close();
            return data;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <param name="strSql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int Insert(this BaseDataProvider dataProvider, string strSql, object param)
        {
            var idbCon = dataProvider.GetIDbConnection();
            var data = idbCon.Execute(strSql, param);
            idbCon.Close();
            return data;
        }
    }
}