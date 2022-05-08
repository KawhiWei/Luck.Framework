namespace Luck.Framework.Event
{
    /// <summary>
    /// 自定义事件继承接口
    /// </summary>
    public interface IIntegrationEvent
    {
        string Id { get; }

        DateTime CreationDate { get; }
    }



}
