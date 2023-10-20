namespace Luck.UnitTest.Pipeline;

public class ContextBase<TRequest, TResponse>
{
    /// <summary>
    /// 请求
    /// </summary>
    public TRequest Request { get; set; } = default!;

    /// <summary>
    /// 响应
    /// </summary>
    public TResponse Response { get; set; } = default!;
}