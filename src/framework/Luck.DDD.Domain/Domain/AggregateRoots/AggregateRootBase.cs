
namespace Luck.DDD.Domain.Domain.AggregateRoots
{
    public class AggregateRootBase : IAggregateRootBase
    {
        private string? _aggregateRootType;


        private string AggregateRootName
        {
            get
            {
                var aggregateRootType = GetType();
                _aggregateRootType = aggregateRootType.FullName;
                if (_aggregateRootType is null)
                {
                    throw new ArgumentNullException(nameof(_aggregateRootType));
                }

                return _aggregateRootType;
            }
        }
        
    }
}
