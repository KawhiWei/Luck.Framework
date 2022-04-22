using Luck.Framework.Infrastructure.DependencyInjectionModule;
using Luck.Framework.Repositories;
using Luck.Framework.UnitOfWorks;
using Module.Sample.Domain;

namespace Module.Sample.Services
{
    public class OrderService: IOrderService
    {

        public readonly IAggregateRootRepository<Order, string> _aggregateRootRepository;
        public readonly IUnitOfWork _unitOfWork;

        public OrderService(IAggregateRootRepository<Order, string> aggregateRootRepository, IUnitOfWork unitOfWork)
        {
            _aggregateRootRepository = aggregateRootRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync()
        {
            var order = new Order("asdasdsa","asdasdadas");
            _aggregateRootRepository.Add(order);
            await _unitOfWork.CommitAsync();
        }
    }
    public interface IOrderService:IScopedDependency
    {
        Task CreateAsync();
    }
}
