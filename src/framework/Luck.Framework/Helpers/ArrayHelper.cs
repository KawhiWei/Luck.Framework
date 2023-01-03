namespace Luck.Framework.Helpers
{

    /// <summary>
    /// 
    /// </summary>
    public static class ArrayHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] Empty<T>() =>
#if NET45

            EmptyArray<T>.Value
#else
            Array.Empty<T>()
#endif
        ;

#if NET45
        private static class EmptyArray<T>
        {
            public static readonly T[] Value = new T[0];
        }
#endif
    }

}
