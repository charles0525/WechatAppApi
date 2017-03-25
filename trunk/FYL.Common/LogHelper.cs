using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;

namespace FYL.Common
{
    public class LogHelper
    {
        private readonly static ILog log = LogManager.GetLogger("FYL.ApiService");

        static LogHelper()
        {
            Init();
        }

        public static void Error(string msg, Exception ex = null)
        {
            log.Error(msg, ex);
        }

        public static void Warning(string msg, Exception ex = null)
        {
            log.Warn(msg, ex);
        }

        public static void Info(string msg, Exception ex = null)
        {
            log.Info(msg, ex);
        }

        private static void Init()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }
    }
}
