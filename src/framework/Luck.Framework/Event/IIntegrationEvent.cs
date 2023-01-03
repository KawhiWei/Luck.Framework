namespace Luck.Framework.Event
{
    /// <summary>
    /// 自定义事件继承接口
    /// </summary>
    public interface IIntegrationEvent
    {
        /// <summary>
        /// 
        /// </summary>
        string EventId { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTime EventCreationDate { get; }
    }



}
