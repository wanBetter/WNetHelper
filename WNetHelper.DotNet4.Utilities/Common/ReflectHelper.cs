﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace WNetHelper.DotNet4.Utilities.Common
{
    /// <summary>
    ///     反射帮助类
    /// </summary>
    public static class ReflectHelper
    {
        #region Fields

        /// <summary>
        ///     方法或属性搜索标志
        /// </summary>
        /// 日期：2015-10-10 9:08
        /// 备注：
        private static readonly BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public |
                                                            BindingFlags.NonPublic | BindingFlags.Instance |
                                                            BindingFlags.Static;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     利用反射来判断对象是否包含某个属性
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="model">实体类对象</param>
        /// <param name="propertyName">需要判断的属性</param>
        /// <returns>是否包含</returns>
        public static bool Contain<T>(T model, string propertyName)
            where T : class
        {
            var propertyInfo = model.GetType().GetProperty(propertyName);
            return propertyInfo != null;
        }

        /// <summary>
        ///     反射创建对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="fullName">类全名称</param>
        /// <param name="assemblyName">程序集全程</param>
        /// <returns>类型</returns>
        /// 时间：2016-01-07 14:47
        /// 备注：
        public static T CreateInstance<T>(string fullName, string assemblyName)
            where T : class, new()
        {
            var path = fullName + "," + assemblyName; //命名空间.类型名,程序集
            var loadType = Type.GetType(path); //加载类型
            var instance = Activator.CreateInstance(loadType ?? throw new InvalidOperationException(), true); //根据类型创建实例
            return (T) instance; //类型转换并返回
        }

        /// <summary>
        ///     获取值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="F">泛型</typeparam>
        /// <param name="model">对象</param>
        /// <param name="propertyName">要获取值的名称</param>
        /// <returns>反射获取到的值</returns>
        /// 日期：2015-10-10 9:07
        /// 备注：
        public static F GetFieldValue<T, F>(T model, string propertyName)
            where T : class
        {
            var fieldInfo = model.GetType().GetField(propertyName, bindingFlags);
            return (F) fieldInfo?.GetValue(model);
        }

        /// <summary>
        ///     反射获取属性集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>属性集合</returns>
        /// 时间：2016/9/1 10:27
        /// 备注：
        public static PropertyInfo[] GetPropertyInfo<T>() where T : class
        {
            return typeof(T).GetProperties();
        }

        /// <summary>
        ///     获取属性名称，优先获取DisplayName
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>属性名称</returns>
        /// 时间：2016/9/1 10:59
        /// 备注：
        public static IDictionary<string, string> GetPropertyName<T>() where T : class
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                dict.Add(property.Name,
                    attributes.Length == 0 ? property.Name : ((DisplayNameAttribute) attributes[0]).DisplayName);
            }

            return dict;
        }

        /// <summary>
        ///     将实体类属性转换为字典形式
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="model">实体类</param>
        /// <returns>字典</returns>
        public static Dictionary<string, object> DictionaryFromType<T>(this T model) where T : class
        {
            var properties = GetPropertyInfo<T>();
            var dict = new Dictionary<string, object>();

            foreach (var item in properties)
            {
                var attris = item.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                var attrName = attris.Length == 0 ? item.Name : ((DisplayNameAttribute) attris[0]).DisplayName;
                var attrValue = item.GetValue(model, new object[] { });
                dict.Add(attrName, attrValue);
            }

            return dict;
        }

        /// <summary>
        ///     反射调用方法
        /// </summary>
        /// <param name="item">需反射类型</param>
        /// <param name="methodName">调用方法名称</param>
        /// <param name="args">参数</param>
        /// <returns>方法返回值</returns>
        public static object InvokeMethod(object item, string methodName, object[] args)
        {
            var type = item.GetType();
            return type.InvokeMember(methodName, bindingFlags | BindingFlags.InvokeMethod, null, item, args);
        }

        /// <summary>
        ///     反射设置值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="F">泛型</typeparam>
        /// <param name="model">操作对象</param>
        /// <param name="name">名称</param>
        /// <param name="fieldValue">值</param>
        public static void SetFieldValue<T, F>(T model, string name, F fieldValue)
            where T : class
        {
            var fieldInfo = model.GetType().GetField(name, bindingFlags);
            fieldInfo?.SetValue(model, fieldValue);
        }

        #endregion Methods
    }
}