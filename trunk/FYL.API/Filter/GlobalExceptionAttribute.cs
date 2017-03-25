using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using FYL.Common;
using System.Net.Http;
using System.Net;
using FYL.API.Models;
using FYL.Entity;

namespace FYL.API.Filter
{
    public class GlobalExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var response = new HttpResponseMessage();
            var exception = context.Exception;
            if (exception is CustomException)
            {
                response.Content = ResponseHelper.Content(Entity.Enum.EnumApiStatusCode.Fail, exception.Message);
                LogHelper.Warning(exception.Message, exception);
            }
            else
            {
                response.Content = ResponseHelper.Content(Entity.Enum.EnumApiStatusCode.Error, $"服务内部错误:{{{context.Exception.Message}}}");
                LogHelper.Error(exception.Message, exception);
            }
            response.StatusCode = HttpStatusCode.OK;
            response.ReasonPhrase = context.Exception.Message;
            context.Response = response;

            base.OnException(context);
        }
    }
}