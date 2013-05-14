using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SnapClutch.Config
{
    public class XmlUtility
    {
        public static void Serialize(Object data, string fileName)
        {
            Type type = data.GetType();
            XmlSerializer xs = new XmlSerializer(type);
            XmlTextWriter xmlWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xs.Serialize(xmlWriter, data);
            xmlWriter.Close();
        }

        public static Object Deserialize(Type type, string fileName)
        {
            XmlSerializer xs = new XmlSerializer(type);

            XmlTextReader xmlReader = new XmlTextReader(fileName);
            Object data = xs.Deserialize(xmlReader);

            xmlReader.Close();

            return data;
        }
    }
}
