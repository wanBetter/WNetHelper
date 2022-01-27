﻿using System;
using System.Linq.Expressions;

namespace WNetHelper.DotNet4.Utilities.Common
{
    /// <summary>
    ///     Expression 辅助类
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        ///     获取Member
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns>名称</returns>
        public static string GetMemberName(this Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression) expression;
                    var supername = GetMemberName(memberExpression.Expression);
                    if (string.IsNullOrEmpty(supername)) return memberExpression.Member.Name;

                    return string.Concat(supername, '.', memberExpression.Member.Name);

                case ExpressionType.Call:
                    var callExpression = (MethodCallExpression) expression;
                    return callExpression.Method.Name;

                case ExpressionType.Convert:
                    var unaryExpression = (UnaryExpression) expression;
                    return GetMemberName(unaryExpression.Operand);

                case ExpressionType.Parameter:
                case ExpressionType.Constant:
                    return string.Empty;

                default:
                    throw new ArgumentException("The expression is not a member access or method call expression");
            }
        }
    }
}