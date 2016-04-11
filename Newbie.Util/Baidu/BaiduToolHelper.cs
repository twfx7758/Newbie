using Newbie.Util.Baidu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Newbie.Util
{
   public class BaiduToolHelper
    {
        public static void CreateSitemapWithXML(urlset urlData, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(urlset));
            FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(stream, urlData, ns);
            stream.Close();
        }
        public static void CreateSitemapIndexWithXML(sitemapindex urlData, string fileName)
        {
            string xml = string.Empty;
            XmlSerializer serializer = new XmlSerializer(typeof(sitemapindex));
            FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(stream, urlData, ns);
            stream.Close();
        }
    }
}
