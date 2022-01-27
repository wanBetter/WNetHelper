﻿namespace WNetHelper.DotNet4.Utilities.Enums
{
    /// <summary>
    /// 图片裁剪方式
    /// </summary>
    public enum ImgCutType
    {
        /// <summary>
        /// 根据高宽剪切
        /// </summary>
        CutWh = 1,

        /// <summary>
        /// 根据宽剪切
        /// </summary>
        CutW = 2,

        /// <summary>
        /// 根据高剪切
        /// </summary>
        CutH = 3,

        /// <summary>
        /// 缩放不剪裁
        /// </summary>
        CutNo = 4
    }
}