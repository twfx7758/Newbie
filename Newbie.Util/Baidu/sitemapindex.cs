using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Newbie.Util.Baidu
{
    [XmlRoot("sitemapindex")]
    public class sitemapindex : List<sitemap>
    {
    }
}
