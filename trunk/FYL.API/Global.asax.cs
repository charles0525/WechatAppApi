using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using FYL.Common;
using System.Text;
using FYL.API.Filter;

namespace FYL.API
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configuration.Filters.Add(new GlobalExceptionAttribute());

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler((sender2, e2) =>
            {
                Exception ex = e2.ExceptionObject as Exception;
                if (ex != null)
                {
                    LogHelper.Error("系统未处理异常", ex);
                }
                else
                {
                    LogHelper.Error("系统未处理异常，并且异常对象为空");
                }
            });
        }
    }
}