using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Newbie.Util
{
    public class MsgModelXmlHelper
    {
        public static XmlNode GetXmlByElement(string XMLpath,string ElementName)
        {
            XmlElement root = null;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(XMLpath);
            root = xmldoc.DocumentElement;


            XmlNodeList nodelist = root.GetElementsByTagName(ElementName);
            if (nodelist.Count > 0)
            {
                return nodelist.Item(0);
            }
            else
            {
                return null;
            }
        }
    }
}
