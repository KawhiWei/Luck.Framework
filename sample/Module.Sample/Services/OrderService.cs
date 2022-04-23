using Luck.DDD.Domain.Repositories;
using Luck.Framework.Infrastructure.DependencyInjectionModule;
using Luck.Framework.UnitOfWorks;
using MediatR;
using Module.Sample.Domain;
using Module.Sample.EventHandlers;

namespace Module.Sample.Services
{
    public class OrderService : IOrderService
    {

        private readonly IAggregateRootRepository<Order, string> _aggregateRootRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public OrderService(IAggregateRootRepository<Order, string> aggregateRootRepository, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _aggregateRootRepository = aggregateRootRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task CreateAsync()
        {
            var order = new Order("asdasdsa", "asdasdadas");
            _aggregateRootRepository.Add(order);
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
            order.AddDomainEvent(new OrderCreatedEto() { Id = order.Id, Name = order.Name });
            await _unitOfWork.CommitAsync();


            //TODO 领域事件是否需要显示的发送出去？？
            foreach (var item in order.GetDomainEvents())
            {
                await _mediator.Publish(item);
            }


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
