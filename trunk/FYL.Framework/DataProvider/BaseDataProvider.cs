using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using FYL.Entity.Enum;
using FYL.Entity;

namespace FYL.Framework.DataProvider
{
    /// <summary>
    /// 数据访问声明
    /// by charles.he
    /// @2016.12.26
    /// </summary>
    public class BaseDataProvider
    {
        private EnumDataProviderType _emProvider;
        public EnumDataProviderType EmProvider { get { return _emProvider; } }

        public BaseDataProvider(EnumDataProviderType? emProvider)
        {
            if (emProvider == null)
            {
                throw new CustomException("数据库枚举未赋值");
            }

            _emProvider = emProvider.Value;
        }

        public IDbConnection GetIDbConnection()
        {
            var strCon = GetConStr();
            IDbConnection con = new MySqlConnection(strCon);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            return con;
        }

        public string GetConStr()
        {
            switch (_emProvider)
            {
                case EnumDataProviderType.PmhMall:
                    return ConfigurationManager.AppSettings["MallConnectionString"];
                case EnumDataProviderType.PmhMobile:
                    return ConfigurationManager.AppSettings["MobileConnectionString"];
                case EnumDataProviderType.PmhMobileHot:
                    return ConfigurationManager.AppSettings["MobileHotConnectionString"];
                default:
                    throw new Exception("获取数据库链接失败");
            }
        }
    }
}