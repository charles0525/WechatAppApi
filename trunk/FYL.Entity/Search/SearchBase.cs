using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYL.Entity.Search
{
    public class SearchBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keywords { get; set; }
    }
}
