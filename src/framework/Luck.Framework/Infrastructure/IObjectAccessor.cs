using JetBrains.Annotations;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public interface IObjectAccessor<TType>
    {
        /// <summary>
        /// 
        /// </summary>
        TType Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public class ObjectAccessor<TType> : IObjectAccessor<TType>
    {
        /// <summary>
        /// 
        /// </summary>
        public ObjectAccessor()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public ObjectAccessor([CanBeNull] TType obj)
        {
            Value = obj;
        }

        /// <summary>
        /// 
        /// </summary>
        public TType Value { get; set; }
    }
}