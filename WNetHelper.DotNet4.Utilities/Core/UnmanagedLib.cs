﻿using System;
using System.Runtime.InteropServices;
using WNetHelper.DotNet4.Utilities.Operator;

namespace WNetHelper.DotNet4.Utilities.Core
{
    /// <summary>
    ///     非托管DLL加载处理
    /// </summary>
    /// 时间：2016/11/7 13:50
    /// 备注：
    public class UnmanagedLib : IDisposable
    {
        #region Constructors

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="libFilePath">非托管DLL路径</param>
        /// 时间：2016/11/7 13:59
        /// 备注：
        public UnmanagedLib(string libFilePath)
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(libFilePath, "非托管DLL路径")
                .IsFilePath(libFilePath).CheckFileExists(libFilePath);
            LibFilePath = libFilePath;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     非托管DLL 路径
        /// </summary>
        public readonly string LibFilePath;

        private IntPtr _instance; //dll实例

        #endregion Fields

        #region Methods

        /// <summary>
        ///     卸载DLL
        /// </summary>
        /// <param name="hModule">hModule</param>
        /// <returns>卸载是否成功</returns>
        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        ///     调用方法指针
        /// </summary>
        /// <param name="hModule">hModule</param>
        /// <param name="lpProcName">lpProcName</param>
        /// <returns>IntPtr</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        /// <summary>
        ///     加载DLL
        /// </summary>
        /// <param name="lpFileName">lpFileName</param>
        /// <returns>IntPtr</returns>
        /// 时间：2016/11/7 13:58
        /// 备注：
        [DllImport("Kernel32.dll")]
        public static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// 时间：2016/11/7 14:10
        /// 备注：
        public void Dispose()
        {
            FreeLib();
        }

        /// <summary>
        ///     释放
        /// </summary>
        public void FreeLib()
        {
            FreeLibrary(_instance);
        }

        /// <summary>
        ///     加载DLL
        /// </summary>
        public void Load()
        {
            _instance = LoadLibrary(LibFilePath);

            if (_instance == IntPtr.Zero) throw new ArgumentException("加载非托管DLL失败！");
        }

        /// <summary>
        ///     获取方法指针
        /// </summary>
        /// <param name="functionName">方法指针名称</param>
        /// <param name="t">类型</param>
        /// <returns>委托</returns>
        private Delegate GetAddress(string functionName, Type t)
        {
            var procAddr = GetProcAddress(_instance, functionName);

            if (procAddr == IntPtr.Zero)
                return null;
            return Marshal.GetDelegateForFunctionPointer(procAddr, t);
        }

        #endregion Methods
    }
}