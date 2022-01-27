﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using WNetHelper.DotNet4.Utilities.Operator;

namespace WNetHelper.DotNet4.Utilities.Common
{
    /// <summary>
    ///     序列化帮助类
    /// </summary>
    public static class SerializeHelper
    {
        #region Methods

        /// <summary>
        ///     反序列化二进制文件
        /// </summary>
        /// <param name="filePath">二进制文件</param>
        /// <returns>对象</returns>
        // ReSharper disable once UnusedMember.Global
        public static T BinaryDeserialize<T>(string filePath)
        {
            ValidateOperator.Begin().CheckFileExists(filePath);
            var buffer = FileHelper.ReadFile(filePath);
            return BinaryDeserialize<T>(buffer);
        }

        /// <summary>
        ///     将使用二进制格式保存的byte数组反序列化成对象
        /// </summary>
        /// <param name="deserializeBuffer">byte数组</param>
        /// <returns>对象</returns>
        public static T BinaryDeserialize<T>(byte[] deserializeBuffer)
        {
            ValidateOperator.Begin().NotNull(deserializeBuffer, "deserializeBuffer");
            using (var stream = new MemoryStream(deserializeBuffer))
            {
                var binarySerializer = new BinaryFormatter();
                return (T) binarySerializer.Deserialize(stream);
            }
        }

        /// <summary>
        ///     将对象使用二进制格式序列化成byte数组
        /// </summary>
        /// <param name="serializeData">需要序列化对象</param>
        /// <returns>byte数组</returns>
        public static byte[] BinarySerialize<T>(T serializeData)
        {
            CheckedSerializeData(serializeData);
            using (var stream = new MemoryStream())
            {
                var binarySerializer = new BinaryFormatter();
                binarySerializer.Serialize(stream, serializeData);
                return stream.ToArray();
            }
        }

        /// <summary>
        ///     将对象序列化成二进制文件保存
        /// </summary>
        /// <param name="serializeData">需要序列化的对象</param>
        /// <param name="saveFilePath">保存文件</param>
        public static void BinarySerialize<T>(T serializeData, string saveFilePath)
        {
            CheckedSerializeData(serializeData);
            ValidateOperator.Begin().IsFilePath(saveFilePath);
            var buffer = BinarySerialize(serializeData);
            FileHelper.SaveFile(buffer, saveFilePath);
        }

        /// <summary>
        ///     利用DataContractSerializer反序列化
        /// </summary>
        /// <param name="deserializeString">需要反序列化字符串</param>
        /// <returns>object</returns>
        public static T DataContractDeserialize<T>(string deserializeString)
        {
            CheckedDeserializeString(deserializeString);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(deserializeString)))
            {
                var dataContractSerializer = new DataContractSerializer(typeof(T));
                return (T) dataContractSerializer.ReadObject(stream);
            }
        }

        /// <summary>
        ///     利用DataContractSerializer对象序列化
        /// </summary>
        /// <param name="serializeData">需要序列化的对象</param>
        /// <returns>字符串</returns>
        public static string DataContractSerialize<T>(T serializeData)
        {
            CheckedSerializeData(serializeData);
            using (var stream = new MemoryStream())
            {
                var dataContractSerializer = new DataContractSerializer(typeof(T));
                dataContractSerializer.WriteObject(stream, serializeData);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        ///     利用JavaScriptSerializer将json字符串反序列化
        /// </summary>
        /// <param name="deserializeString">Json字符串</param>
        /// <returns>object</returns>
        public static T JsonDeserialize<T>(string deserializeString)
        {
            CheckedDeserializeString(deserializeString);
            var jsonHelper = new JavaScriptSerializer();
            return jsonHelper.Deserialize<T>(deserializeString);
        }

        /// <summary>
        ///     利用JavaScriptSerializer将对象序列化成JSON字符串
        /// </summary>
        /// <param name="serializeData">需要序列化的对象</param>
        /// <param name="scriptConverters">JavaScriptConverter</param>
        /// <returns>Json字符串</returns>
        public static string JsonSerialize<T>(T serializeData, params JavaScriptConverter[] scriptConverters)
        {
            CheckedSerializeData(serializeData);
            var jsonHelper = new JavaScriptSerializer();

            if (scriptConverters != null) jsonHelper.RegisterConverters(scriptConverters);

            jsonHelper.MaxJsonLength = int.MaxValue;
            return jsonHelper.Serialize(serializeData);
        }

        /// <summary>
        ///     利用JavaScriptSerializer将对象序列化成JSON字符串
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="serializeData">需要序列化的对象</param>
        /// <returns>Json字符串</returns>
        public static string JsonSerialize<T>(T serializeData)
        {
            return JsonSerialize(serializeData, null);
        }

        /// <summary>
        ///     处理JsonString的时间格式问题【时间格式：yyyy-MM-dd HH:mm:ss】
        /// </summary>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>处理好的Json字符串</returns>
        public static string ParseJsonDateTime(this string jsonString) => ParseJsonDateTime(jsonString, "yyyy-MM-dd HH:mm:ss");

        /// <summary>
        ///     处理JsonString的时间格式问题
        ///     <para>
        ///         eg:ScriptSerializerHelper.ConvertTimeJson(@"[{'getTime':'\/Date(1419564257428)\/'}]", "yyyyMMdd
        ///         hh:mm:ss");==>[{'getTime':'20141226 11:24:17'}]
        ///     </para>
        /// </summary>
        /// <param name="jsonString">Json字符串</param>
        /// <param name="formart">时间格式化类型</param>
        /// <returns>处理好的Json字符串</returns>
        public static string ParseJsonDateTime(this string jsonString, string formart)
        {
            if (!string.IsNullOrEmpty(jsonString))
                jsonString = Regex.Replace(
                    jsonString,
                    @"\\/Date\((\d+)\)\\/",
                    match =>
                    {
                        var dateTime = new DateTime(1970, 1, 1);
                        dateTime = dateTime.AddMilliseconds(long.Parse(match.Groups[1].Value));
                        dateTime = dateTime.ToLocalTime();
                        return dateTime.ToString(formart);
                    });

            return jsonString;
        }

        /// <summary>
        ///     利用XmlSerializer来反序列化
        /// </summary>
        /// <param name="deserializeStringOrPath">需要反序列化的字符串或者路径</param>
        /// <returns>对象</returns>
        public static T XmlDeserialize<T>(string deserializeStringOrPath)
        {
            CheckedDeserializeString(deserializeStringOrPath);
            if (File.Exists(deserializeStringOrPath))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(deserializeStringOrPath);
                deserializeStringOrPath = xmlDoc.InnerXml.Trim();
            }

            var xmlSerializer = new XmlSerializer(typeof(T));
            var writer = new StringReader(deserializeStringOrPath);
            return (T) xmlSerializer.Deserialize(writer);
        }

        /// <summary>
        ///     序列化，使用标准的XmlSerializer
        ///     不能序列化IDictionary接口.
        /// </summary>
        /// <param name="serializeData">需要序列化的对象</param>
        /// <param name="filename">文件路径</param>
        public static void XmlSerialize<T>(T serializeData, string filename)
        {
            CheckedSerializeData(serializeData);
            ValidateOperator.Begin().IsFilePath(filename);
            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                var xmlSerializer = new XmlSerializer(serializeData.GetType());
                xmlSerializer.Serialize(stream, serializeData);
            }
        }

        /// <summary>
        ///     序列化，使用标准的XmlSerializer
        ///     不能序列化IDictionary接口.
        /// </summary>
        /// <param name="serializeData">需要序列化的对象</param>
        /// <returns>xml字符串</returns>
        public static string XmlSerialize<T>(T serializeData)
        {
            CheckedSerializeData(serializeData);
            var xmlSerializer = new XmlSerializer(typeof(T));
            var writer = new StringWriter();
            xmlSerializer.Serialize(writer, serializeData);
            return writer.ToString();
        }

        /// <summary>
        ///     序列化，使用标准的XmlSerializer
        ///     不能序列化IDictionary接口.
        /// </summary>
        /// <param name="serializeData">需要序列化的对象</param>
        /// <param name="encoding">xml字符串</param>
        /// <returns>xml字符串</returns>
        public static string XmlSerialize<T>(T serializeData, Encoding encoding)
        {
            CheckedSerializeData(serializeData);
            using (var stream = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(T));
                var writer = new StreamWriter(stream, encoding);
                var xsn = new XmlSerializerNamespaces();
                xsn.Add(string.Empty, string.Empty);
                serializer.Serialize(writer, serializeData, xsn);
                return encoding.GetString(stream.ToArray());
            }
        }

        private static void CheckedDeserializeString(string deserializeString)
        {
            ValidateOperator.Begin().NotNull(deserializeString, "deserializeString");
        }

        private static void CheckedSerializeData<T>(T serializeData)
        {
            ValidateOperator.Begin().NotNull(serializeData, "serializeData");
        }

        #endregion Methods
    }
}