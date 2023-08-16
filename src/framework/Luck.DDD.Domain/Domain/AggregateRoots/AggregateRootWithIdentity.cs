namespace Luck.DDD.Domain.Domain.AggregateRoots
{
    public class AggregateRootWithIdentity<TKey> : AggregateRootBase
    {
        public AggregateRootWithIdentity(TKey id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            Id = id;
        }
        public TKey Id { get; }
        /// <summary>
        /// 重写Equals方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is AggregateRootWithIdentity<TKey> entity))//判断obj是否是派生自EntityBase
            {
                return false;
            }
            return base.Equals(obj);
        }
        /// <summary>
        /// 重写HashCode方法
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
