using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYL.API.Models
{
    public class UpdateOrderReq : RequestBase
    {
        public long orderNo { get; set; }
    }
}