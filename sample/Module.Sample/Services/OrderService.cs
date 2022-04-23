using Luck.DDD.Domain.Repositories;
using Luck.Framework.Infrastructure.DependencyInjectionModule;
using Luck.Framework.UnitOfWorks;
using Module.Sample.Domain;
using Module.Sample.EventHandlers;

namespace Module.Sample.Services
{
    public class OrderService : IOrderService
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
            var order = new Order("asdasdsa", "asdasdadas");
            _aggregateRootRepository.Add(order);
            order.AddDomainEvent(new OrderCreatedEto());
            await _unitOfWork.CommitAsync();
        }


        /// <summary>
        /// 创建与发布事件
        /// </summary>
        /// <returns></returns>
        public async Task CreateAndEventAsync()
        {
            var order = new Order("asdasdsa", "asdasdadas");
            _aggregateRootRepository.Add(order);
            order.AddDomainEvent(new OrderCreatedEto() { Id= order.Id,Name= order .Name});
            await _unitOfWork.CommitAsync();
        }
    }
    public interface IOrderService : IScopedDependency
    {
        Task CreateAsync();

        /// <summary>
        /// 创建与发布事件
        /// </summary>
        /// <returns></returns>
        Task CreateAndEventAsync();
    }
}
