using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace UtilityTools.DataStructureManager
{
    public class XmlManager
    {
        public static T DeserializeFromPath<T>(string pathXml, XmlRootAttribute root = null)
        {
            if (string.IsNullOrEmpty(pathXml))
                return default;

            if (!Directory.Exists(pathXml))
                return default;

            T retObj = default;
            using (FileStream reader = new FileStream(pathXml, FileMode.Open))
            {
                retObj = XmlManager.Deserialize<T>(reader, root);
            }

            return retObj;
        }
        public static T DeserializeFromString<T>(string xmlString, XmlRootAttribute root = null)
        {
            if (string.IsNullOrEmpty(xmlString))
                return default;

            T retObj = default;
            using (StringReader sr = new StringReader(xmlString))
            {
                retObj = XmlManager.Deserialize<T>(sr, root);
            }
            //byte[] xmlByte = Encoding.UTF8.GetBytes(xmlString);
            //using (MemoryStream ms = new MemoryStream(xmlByte))
            //{
            //    retObj = XmlManager.Deserialize<T>(ms, root);
            //}
            return retObj;
        }
        public static T Deserialize<T>(TextReader reader, XmlRootAttribute root = null)
        {
            if (reader == null)
                return default;

            T retObj = default;
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);
            retObj = (T)serializer.Deserialize(reader);

            return retObj;
        }
        public static T Deserialize<T>(Stream stream, XmlRootAttribute root = null)
        {
            if (stream == null)
                return default;

            T retObj = default;
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);
            retObj = (T)serializer.Deserialize(stream);

            return retObj;
        }
    }
}
