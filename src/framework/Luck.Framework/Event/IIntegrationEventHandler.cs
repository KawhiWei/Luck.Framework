

namespace Luck.Framework.Event
{

    /// <summary>
    /// 集成事件处理器
    /// </summary>
    public interface IIntegrationEventHandler<in TIntegrationEvent>
          where TIntegrationEvent : IIntegrationEvent
    {

        /// <summary>
        /// 处理器(要不要加Async结尾)
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task HandleAsync(TIntegrationEvent @event);
    }
}
