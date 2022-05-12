namespace Luck.Framework.Event
{
    /// <summary>
    /// 自定义事件继承接口
    /// </summary>
    public interface IIntegrationEvent
    {
        string EventId { get; }

        DateTime EventCreationDate { get; }
    }



}
