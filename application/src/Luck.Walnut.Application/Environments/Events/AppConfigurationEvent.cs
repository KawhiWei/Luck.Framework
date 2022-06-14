using MediatR;

namespace Luck.Walnut.Application.Environments.Events;

/// <summary>
/// 内存事件通知
/// </summary>
public class AppConfigurationEvent : INotification
{
    public string Id { get; set; } = default!;
}
