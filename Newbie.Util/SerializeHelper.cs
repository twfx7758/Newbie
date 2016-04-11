using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;

namespace Newbie.Util
{
    public class SerializeHelper
    {
        #region XML序列化
        /// <summary>
        /// 文件化XML序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void Save(object obj, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }
        /// <summary>
        /// 文件化XML反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        public static object Load(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        /// 文本化XML序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToXml<T>(T item)
        {
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                serializer.Serialize(writer, item);
                return sb.ToString();
            }
        }

        /// <summary>
        /// 文本化XML反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T FromXml<T>(string str)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (XmlReader reader = new XmlTextReader(new StringReader(str)))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
        public static T FromXml<T>(string xml, Encoding encode)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
        public static T Deserialize<T>(string file)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(file, settings);
            T list = default(T);
            if (ser.CanDeserialize(reader))
                list = (T)ser.Deserialize(reader);
            return list;
        }
        public void Serializer<T>(string file, T list)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            ser.Serialize(stream, list);
            stream.Close();
        }

        #region 补充
        ///  <summary>
        ///  将字符串内容反序列化成对象
        ///  使用:BlogSettingInfo  info  =  (BlogSettingInfo)Serializer.XmlDeserializerFormText(typeof(BlogSettingInfo),config);
        ///  </summary>
        ///  <param  name="type">对象类型</param>
        ///  <param  name="serializeText">被序列化的文本</param>
        ///  <returns></returns>
        public static object XmlDeserializerFormText(Type type, string serializeText)
        {
            serializeText = ReplaceLowOrderASCIICharacters(serializeText);
            using (StringReader reader = new StringReader(serializeText))
            {
                return new XmlSerializer(type).Deserialize(reader);
            }
        }

        ///  <summary>
        ///  将目标对象序列化成完整的XML文档
        ///  </summary>
        ///  <param  name="target"></param>
        ///  <returns></returns>
        public static string XmlSerializerToXml(object target)
        {
            return XmlSerializerToText(target, false);
        }

        ///  <summary>
        ///  将目标对象序列化成XML文档内容(去除声明属性)
        ///  </summary>
        ///  <param  name="target"></param>
        ///  <returns></returns>
        public static string XmlSerializerToText(object target)
        {
            return XmlSerializerToText(target, true);
        }

        ///  <summary>
        ///  将目标对象序列化成XML文档
        ///  </summary>
        ///  <param  name="target"></param>
        ///  <param  name="isText">是否去除声明属性</param>
        ///  <returns></returns>
        private static string XmlSerializerToText(object target, bool isText)
        {
            StringBuilder sb = new StringBuilder();
            //去除开头的<?xml version="1.0" encoding="utf-8"?>
            //XmlWriterSettings settings = new XmlWriterSettings();
            //settings.OmitXmlDeclaration = false;
            //添加开头的<?xml version="1.0" encoding="utf-8"?>

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = isText ? false : true;
            settings.Encoding = Encoding.UTF8;
            //换行，缩进
            settings.Indent = true;
            //settings.IndentChars = "\t";
            //using (FileStream writer = new FileStream(strFile, FileMode.Create))
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                XmlSerializer formatter = new XmlSerializer(target.GetType());
                if (isText)
                {
                    //去除默认命名空间xmlns:xsd和xmlns:xsi
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    formatter.Serialize(writer, target, ns);
                }
                else
                {
                    formatter.Serialize(writer, target);
                }
            }
            return sb.ToString();
        }


        public static string ReplaceLowOrderASCIICharacters(string tmp)
        {
            StringBuilder info = new StringBuilder();
            foreach (char cc in tmp)
            {
                int ss = (int)cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                    info.AppendFormat(" ", ss);//&#x{0:X};
                else info.Append(cc);
            }
            return info.ToString();
        }
        #endregion

        #endregion

        #region Json序列化
        /// <summary>
        /// JsonSerializer序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToJson<T>(T item)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, item);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// JsonSerializer反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T FromJson<T>(string str) where T : class
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                return serializer.ReadObject(ms) as T;
            }
        }
        #endregion

        #region SoapFormatter序列化
        /// <summary>
        /// SoapFormatter序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToSoap<T>(T item)
        {
            SoapFormatter formatter = new SoapFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                ms.Position = 0;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ms);
                return xmlDoc.InnerXml;
            }
        }

        /// <summary>
        /// SoapFormatter反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T FromSoap<T>(string str)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);
            SoapFormatter formatter = new SoapFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                xmlDoc.Save(ms);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
        #endregion

        #region BinaryFormatter序列化
        /// <summary>
        /// BinaryFormatter序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToBinary<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                ms.Position = 0;
                byte[] bytes = ms.ToArray();
                StringBuilder sb = new StringBuilder();
                foreach (byte bt in bytes)
                {
                    sb.Append(string.Format("{0:X2}", bt));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// BinaryFormatter反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T FromBinary<T>(string str)
        {
            int intLen = str.Length / 2;
            byte[] bytes = new byte[intLen];
            for (int i = 0; i < intLen; i++)
            {
                int ibyte = Convert.ToInt32(str.Substring(i * 2, 2), 16);
                bytes[i] = (byte)ibyte;
            }
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return (T)formatter.Deserialize(ms);
            }
        }
        #endregion


        #region 其它操作


        #region 将对象转化成二进制的数组
        /// <summary>
        /// 将对象转化成二进制的数组
        /// </summary>
        /// <param name="objectToConvert">用于转化的对象。</param>
        /// <returns>返回转化后的数组，如果CanBinarySerialize为false则返回null。</returns>
        public static byte[] ConvertToBytes(object objectToConvert)
        {
            byte[] byteArray = null;


            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {

                binaryFormatter.Serialize(ms, objectToConvert);
                //设置是内存存储位置为0。
                ms.Position = 0;
                //读取数组。
                byteArray = new Byte[ms.Length];
                ms.Read(byteArray, 0, byteArray.Length);
                ms.Close();
            }

            return byteArray;
        }
        #endregion

        #region 将一个二进制的数组转化为对象
        /// <summary>
        /// 将一个二进制的数组转化为对象，必须通过类型转化自己想得到的相应对象。如果数组为空则返回空。
        /// </summary>
        /// <param name="byteArray">用于转化的二进制数组。</param>
        /// <returns>返回转化后的对象实例，如果数组为空，则返回空对象。</returns>
        public static object ConvertToObject(byte[] byteArray)
        {
            object convertedObject = null;
            if (byteArray != null && byteArray.Length > 0)
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(byteArray, 0, byteArray.Length);

                    ms.Position = 0;

                    if (byteArray.Length > 4)
                        convertedObject = binaryFormatter.Deserialize(ms);

                    ms.Close();
                }
            }
            return convertedObject;
        }
        #endregion

        #region 将对象以二进制形式存储到硬盘中
        /// <summary>
        /// 将对象以二进制形式存储到硬盘中
        /// </summary>
        /// <param name="objectToSave">用于保存的对象</param>
        /// <param name="path">文件路径</param>
        /// <returns>如果存储成功则返回true，否则返回false。</returns>
        public static bool SaveAsBinary(object objectToSave, string path)
        {
            if (objectToSave != null)
            {
                byte[] ba = ConvertToBytes(objectToSave);
                if (ba != null)
                {
                    using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fs))
                        {
                            bw.Write(ba);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region 将对象序列化后以XML格式存储于文件中
        /// <summary>
        /// 将对象序列化后以XML格式存储于文件中
        /// </summary>
        /// <param name="objectToConvert">用于序列化的对象</param>
        /// <param name="path">用于存储的文件路径</param>
        public static void SaveAsXML(object objectToConvert, string path)
        {

            if (objectToConvert != null)
            {
                Type t = objectToConvert.GetType();

                XmlSerializer ser = new XmlSerializer(t);

                try
                {
                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        ser.Serialize(writer, objectToConvert);
                        writer.Close();
                    }
                }
                catch (Exception ex)
                {
                }
            }

        }
        #endregion

        #region 将文件的数据转化为对象
        /// <summary>
        /// 将文件的数据转化为对象
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="objectType">希望得到的对象</param>
        /// <returns>返回反序列化的对象</returns>
        public static object ConvertFileToObject(string path, Type objectType)
        {
            object convertedObject = null;

            if (path != null && path.Length > 0)
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer ser = new XmlSerializer(objectType);
                    convertedObject = ser.Deserialize(fs);
                    fs.Close();
                }
            }
            return convertedObject;
        }
        #endregion



        #endregion

    }
}
