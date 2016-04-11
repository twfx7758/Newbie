using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Newbie.Util.Baidu
{
    [XmlRoot("urlset")]
    public class urlset : List<url>
    {

    }
}
