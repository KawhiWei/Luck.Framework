using Luck.Framework.Extensions;


namespace Luck.Framework.Utilities
{

    /// <summary>
    /// 检查
    /// </summary>
    public static class Check
    {

     
        /// <summary>
        /// 检查参数不能为空引用，否则抛出<see cref="ArgumentNullException"/>异常。
        /// </summary>
        /// <param name="value">要验证的值</param>
        /// <param name="paramName">参数名称</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static T NotNull<T>(T value, string paramName)
        {
            ObjectExtension.Require<ArgumentNullException>(value != null, paramName);
            return value;
        }




        /// <summary>
        /// 检查字符串不能为空引用或空字符串，否则抛出<see cref="ArgumentNullException"/>异常或<see cref="ArgumentException"/>异常。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramName">参数名称。</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string NotNullOrEmpty(string value, string paramName)
        {
            ObjectExtension.Require<ArgumentException>(!string.IsNullOrEmpty(value), paramName);
            return value;
        }

    



    }
}
