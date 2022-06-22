using MediatR;

namespace Luck.Walnut.Application.Environments.Events;

/// <summary>
/// 内存事件通知
/// </summary>
public class AppConfigurationEvent : INotification
{
    
    /// <summary>
    /// 环境名称
    /// </summary>
    public string EnvironmentName { get; set; } = default!;
    
    /// <summary>
    /// 应用Id
    /// </summary>
    public string AppId { get; set; } = default!;
}
