namespace Luck.DDD.Domain
{
    public class EntityWithIdentity<TIdentityKey> : Entity
    {
        protected EntityWithIdentity(TIdentityKey id)
        {
            Id = id;
        }

        public TIdentityKey Id { get; }
    }

}
