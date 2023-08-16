using Luck.DDD.Domain.Domian.Entities;

namespace Luck.DDD.Domain.Domain.Entities
{
    public abstract class EntityWithIdentity<TIdentityKey> : Entity
    {
        protected EntityWithIdentity(TIdentityKey id)
        {
            Id = id;
        }

        public TIdentityKey Id { get; }
    }

}
