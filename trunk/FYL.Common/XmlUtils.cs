using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Xml.Serialization;

namespace FYL.Common
{
    /// <summary>
    /// Xml序列化与反序列化
    /// </summary>
    public class XmlUtils
    {
        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml) where T : class
        {
            var type = typeof(T);
            var rooName = System.Text.RegularExpressions.Regex.Match(xml, @"^<(?<root>[\s\S]*?)>")?.Groups["root"]?.Value;
            if (!string.IsNullOrEmpty(rooName))
            {
                var typeName = type.Name;
                xml = xml.Replace($"<{rooName}>", $"<{typeName}>").Replace($"</{rooName}>", $"</{typeName}>");
            }

            if (xml.IndexOf("<![CDATA[") >= 0)
            {
                xml = xml.Replace("<![CDATA[", "").Replace("]]>", "");
            }

            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(type);
                return xmldes.Deserialize(sr) as T;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream stream) where T : class
        {
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            String xml = System.Text.Encoding.UTF8.GetString(bytes);
            return Deserialize<T>(xml);
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        #endregion
    }
}