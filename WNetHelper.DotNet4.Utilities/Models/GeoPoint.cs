﻿namespace WNetHelper.DotNet4.Utilities.Models
{
    /// <summary>
    /// 地图X,Y坐标轴
    /// </summary>
    public class GeoPoint
    {
        #region Fields

        /// <summary>
        /// X轴
        /// </summary>
        public int X;

        /// <summary>
        /// Y轴
        /// </summary>
        public int Y;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public GeoPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        #endregion Constructors
    }
}