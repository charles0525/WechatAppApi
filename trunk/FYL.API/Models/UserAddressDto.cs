using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYL.API.Models
{
    public class UserAddressDto : RequestBase
    {
        public string address { get; set; }
        public int city { get; set; }
        public string mobile { get; set; }
        public string postCode { get; set; }
        public int province { get; set; }
        public string realName { get; set; }
        public string remark { get; set; }
        public string tel { get; set; }
    }

    public class UpdateUserAddressReq : UserAddressDto
    {
    }
}