﻿using System;
using System.Drawing;
using System.Text;

namespace WNetHelper.DotNet4.Utilities.Common
{
    /// <summary>
    ///     Random的帮助类
    /// </summary>
    public static class RandomHelper
    {
        #region Constructors

        /// <summary>
        ///     静态构造函数
        /// </summary>
        static RandomHelper()
        {
            RandomSeed = new Random((int) DateTime.Now.Ticks);
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     随机种子
        /// </summary>
        private static readonly Random RandomSeed;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     生成随机字符串
        /// </summary>
        /// <param name="size">字符串长度</param>
        /// <param name="lowerCase">字符串是小写</param>
        /// <returns>随机字符串</returns>
        public static string NextString(int size, bool lowerCase)
        {
            var builder = new StringBuilder(size);
            var startChar = lowerCase ? 97 : 65; //65 = A / 97 = a

            for (var i = 0; i < size; i++) builder.Append((char) (26 * RandomSeed.NextDouble() + startChar));

            return builder.ToString();
        }

        /// <summary>
        ///     依据指定字符串来生成随机字符串
        /// </summary>
        /// <param name="randomString">指定字符串</param>
        /// <param name="size">字符串长度</param>
        /// <param name="lowerCase">字符串是小写</param>
        /// <returns>随机字符串</returns>
        public static string NextString(string randomString, int size, bool lowerCase)
        {
            var nextString = string.Empty;

            if (!string.IsNullOrEmpty(randomString))
            {
                var builder = new StringBuilder(size);
                var maxCount = randomString.Length - 1;

                for (var i = 0; i < size; i++)
                {
                    var number = RandomSeed.Next(0, maxCount);
                    builder.Append(randomString[number]);
                }

                nextString = builder.ToString();
            }

            return lowerCase ? nextString.ToLower() : nextString.ToUpper();
        }

        /// <summary>
        ///     随机布尔值
        /// </summary>
        /// <returns>随机布尔值</returns>
        public static bool NextBool()
        {
            return RandomSeed.NextDouble() >= 0.5;
        }

        /// <summary>
        ///     生成随机颜色
        /// </summary>
        /// <returns>随机颜色</returns>
        public static Color NextColor()
        {
            return Color.FromArgb((byte) RandomSeed.Next(255), (byte) RandomSeed.Next(255),
                (byte) RandomSeed.Next(255));
        }

        /// <summary>
        ///     获取随机时间
        /// </summary>
        /// <returns>随机时间</returns>
        public static DateTime NextDateTime()
        {
            int year = RandomSeed.Next(1900, DateTime.Now.Year),
                month = RandomSeed.Next(1, 12),
                day = RandomSeed.Next(1, DateTime.DaysInMonth(year, month)),
                hour = RandomSeed.Next(0, 23),
                minute = RandomSeed.Next(0, 59),
                second = RandomSeed.Next(0, 59);
            var dateTimeString = $"{year}-{month}-{day} {hour}:{minute}:{second}";
            return Convert.ToDateTime(dateTimeString);
        }

        /// <summary>
        ///     生成设置范围内的Double的随机数
        ///     <para>eg:RandomHelper.NextDouble(1.5, 1.7);</para>
        /// </summary>
        /// <param name="miniDouble">生成随机数的最大值</param>
        /// <param name="maxiDouble">生成随机数的最小值</param>
        /// <returns>Double的随机数</returns>
        public static double NextDouble(double miniDouble, double maxiDouble)
        {
            return RandomSeed.NextDouble() * (maxiDouble - miniDouble) + miniDouble;
        }

        /// <summary>
        ///     生成随机十六进制字符串
        /// </summary>
        /// <param name="byteLength">生成随机十六进制字节长度</param>
        /// <returns>十六进制字符串</returns>
        /// 时间：2016/11/23 16:45
        /// 备注：
        public static string NextHexString(ushort byteLength)
        {
            var buffer = new byte[byteLength];
            RandomSeed.NextBytes(buffer);
            var builder = new StringBuilder();

            foreach (var x in buffer) builder.Append(x.ToString("X2"));

            return builder.ToString();
        }

        /// <summary>
        ///     生成随机MAC地址
        ///     <para>eg:RandomHelper.NextMacAddress();</para>
        /// </summary>
        /// <returns>新的MAC地址</returns>
        public static string NextMacAddress()
        {
            int minValue = 0, maxValue = 16;
            return
                $"{RandomSeed.Next(minValue, maxValue):x}{RandomSeed.Next(minValue, maxValue):x}:{RandomSeed.Next(minValue, maxValue):x}{RandomSeed.Next(minValue, maxValue):x}:{RandomSeed.Next(minValue, maxValue):x}{RandomSeed.Next(minValue, maxValue):x}:{RandomSeed.Next(minValue, maxValue):x}{RandomSeed.Next(minValue, maxValue):x}:{RandomSeed.Next(minValue, maxValue):x}{RandomSeed.Next(minValue, maxValue):x}:{RandomSeed.Next(minValue, maxValue):x}{RandomSeed.Next(minValue, maxValue):x}"
                    .ToUpper();
        }

        /// <summary>
        ///     生成随机数【包括上下限】
        /// </summary>
        /// <param name="low">下限</param>
        /// <param name="high">上线</param>
        /// <returns>随机数</returns>
        public static int NextNumber(int low, int high)
        {
            return RandomSeed.Next(low, high);
        }

        /// <summary>
        ///     生成随机时间
        ///     <para>eg:RandomHelper.NextTime();</para>
        /// </summary>
        /// <returns>随机时间</returns>
        public static DateTime NextTime()
        {
            var hour = RandomSeed.Next(0, 23);
            var minute = RandomSeed.Next(0, 60);
            var second = RandomSeed.Next(0, 60);
            string dateTimeString = $"{DateTime.Now:yyyy-MM-dd} {hour}:{minute}:{second}";
            return Convert.ToDateTime(dateTimeString);
        }

        #endregion Methods
    }
}