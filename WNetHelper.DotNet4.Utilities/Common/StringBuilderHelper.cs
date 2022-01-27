﻿namespace WNetHelper.DotNet4.Utilities.Common
{
    using System;
    using System.Text;

    /// <summary>
    /// StringBuilder 帮助类
    /// </summary>
    public static class StringBuilderHelper
    {
        #region Methods

        /// <summary>
        /// 清空StringBuilder
        /// </summary>
        /// <param name="builder">StringBuilder</param>
        public static void Clear(this StringBuilder builder)
        {
            if (builder != null)
            {
                builder.Length = 0;
                builder.Capacity = 0;
            }
        }

        /// <summary>
        /// 检查StringBuilder是否为null，当为null的时候，实例化返回。
        /// </summary>
        /// <param name="builder">StringBuilder</param>
        /// <returns>StringBuilder</returns>
        public static StringBuilder NullOrCreate(this StringBuilder builder)
        {
            return builder ?? (new StringBuilder());
        }

        /// <summary>
        /// 移除最后一个字符；
        /// <para>
        ///StringBuilder _builder = new StringBuilder();
        /// _builder.Append("Hello World;");
        /// _builder = _builder.RemoveLast(";");
        /// Assert.AreEqual("Hello World", _builder.ToString());
        /// </para>
        /// </summary>
        /// <param name="builder">StringBuilder</param>
        /// <param name="value">字符</param>
        /// <returns></returns>
        public static StringBuilder RemoveLast(this StringBuilder builder, string value)
        {
            if (builder.Length < 1)
            {
                return builder;
            }

            builder.Remove(builder.ToString().LastIndexOf(value, StringComparison.OrdinalIgnoreCase), value.Length);
            return builder;
        }

        #endregion Methods
    }
}