using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYL.API.Models
{
    public class OrderPayReq : RequestBase
    {
        public string orderNo { get; set; }
    }

    public class OrderPayUpdateRequest : RequestBase
    {
        public string orderNo { get; set; }
        //public double totalFee { get; set; }
        public string buyerEmail { get; set; }
        public string sellerEmail { get; set; }

    }

    public class OrderNotifyUrlReq
    {
        public string return_code { get; set; }
        public string return_msg { get; set; }

        public string result_code { get; set; }
        public string err_code { get; set; }
        public string err_code_des { get; set; }

        public string appid { get; set; }
        public string mch_id { get; set; }
        public string nonce_str { get; set; }
        public string sign { get; set; }
        public string openid { get; set; }
        public string trade_type { get; set; }
        public string bank_type { get; set; }
        public string total_fee { get; set; }
        public string cash_fee { get; set; }
        public string transaction_id { get; set; }
        public string out_trade_no { get; set; }
        public string time_end { get; set; }
    }
}