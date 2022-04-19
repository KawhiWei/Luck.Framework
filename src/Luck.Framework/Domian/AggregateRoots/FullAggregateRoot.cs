using Luck.Framework.Infrastructure;

namespace Luck.Framework.Domian
{
    public class FullAggregateRoot : AggregateRootWithIdentity<string>, IUpdatable, ISoftDeletable
    {
        public FullAggregateRoot() : base(ObjectId.GenerateNewStringId())
        {

        }

        public DateTimeOffset CreationTime { get; private set; }

        public DateTimeOffset? LastModificationTime { get; private set; }

        public DateTimeOffset? DeletionTime { get; private set; }


        public void UpdateCreation()
        {
            CreationTime = DateTimeOffset.UtcNow;
        }

        public void UpdateModification()
        {
            LastModificationTime = DateTimeOffset.UtcNow;
        }

        public void UpdateDeletion()
        {
            DeletionTime = DateTimeOffset.UtcNow;
        }
    }
}
