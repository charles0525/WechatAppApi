using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYL.API.Models
{
    public class UserAddressProvinceDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public IEnumerable<UserAddressCity> citys { get; set; }
    }

    public class UserAddressCity
    {
        //public int parentId { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }
}