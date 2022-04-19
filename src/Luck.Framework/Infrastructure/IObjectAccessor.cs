using JetBrains.Annotations;

namespace Luck.Framework.Infrastructure
{
    public interface IObjectAccessor<TType>
    {
        TType Value { get; set; }
    }

    public class ObjectAccessor<TType> : IObjectAccessor<TType>
    {
        public ObjectAccessor()
        {
        }

        public ObjectAccessor([CanBeNull] TType obj)
        {
            Value = obj;
        }

        public TType Value { get; set; }
    }
}