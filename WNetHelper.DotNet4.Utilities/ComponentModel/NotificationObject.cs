﻿using System.ComponentModel;

namespace WNetHelper.DotNet4.Utilities.ComponentModel
{
    /// <summary>
    ///通知界面属性变更辅助类
    /// </summary>
    public class NotificationObject : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// 属性通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        public void NotifyChanges(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods
    }
}