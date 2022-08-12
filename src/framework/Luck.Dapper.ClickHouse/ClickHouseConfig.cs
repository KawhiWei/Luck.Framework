namespace Luck.Dapper.ClickHouse;

public class ClickHouseConfig
{
    /// <summary>
    /// 主机
    /// </summary>
    public string Host { get; set; } = default!;
    
    /// <summary>
    /// 端口
    /// </summary>
    public ushort Port { get;set; } = 9000;
    
    /// <summary>
    /// 用户名
    /// </summary>
    public string User { get;set; } = default!;
    
    /// <summary>
    /// 密码
    /// </summary>
    
    public string Password { get;set; } = default!;
    
    /// <summary>
    /// 数据库名称
    /// </summary>
    public string Database { get;set; } = default!;
    
    /// <summary>
    /// 读取和写入超时时间
    /// </summary>
    public int ReadWriteTimeout { get;set; } = 10000;
    
    
    /// <summary>
    /// 缓冲区大小
    /// </summary>
    public int BufferSize { get;set; } = 4096;
    
    
    /// <summary>
    /// 是否压缩
    /// </summary>
    public bool Compress { get;set; } = true;
}