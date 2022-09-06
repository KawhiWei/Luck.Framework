namespace Luck.Dove.Logging;

public class DoveLoggerModule
{
    /// <summary>
    /// 类别
    /// </summary>
    public string CategoryName { get; set; } = default!;
    
    /// <summary>
    /// 调用方法
    /// </summary>
    public string Method { get; set; } = default!;
    
    /// <summary>
    /// 日志内容
    /// </summary>
    public string Body { get; set; } = default!;
    
    /// <summary>
    /// 业务唯一过滤
    /// </summary>
    public string BusinessFilter { get; set; } = default!;
    
    /// <summary>
    /// 异常消息
    /// </summary>
    public Exception? Exception { get; set; } = default!;
    
}