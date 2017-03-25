using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYL.API.Models
{
    public class RequestBase
    {
        //public string openId { get;set;}
        public string token { get; set; }
        public string deviceKey { get; set; }
    }
}