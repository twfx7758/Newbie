using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newbie.Util.Baidu
{
    [Serializable]
    public class url
    {
        public string loc { set; get; }
        public string lastmod { set; get; }
        public string changefreq { set; get; }
        public string priority { set; get; }
    }
}
