namespace Luck.Dapper.ClickHouse;

public class ClickHouseConnectionConfig
{
    /// <summary>
    /// 主机地址
    /// </summary>
    public List<ConnectionStringOptions> ConnectionOptionList { get; set; } = new List<ConnectionStringOptions>();

    /// <summary>
    /// 是否是集群部署
    /// </summary>
    public bool IsCluster { get; set; } = false;
}

public class ConnectionStringOptions
{
    /// <summary>
    /// 主机
    /// </summary>
    public string Host { get; set; } = default!;

    /// <summary>
    /// 用户名
    /// </summary>
    public string User { get; set; } = default!;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = default!;

    /// <summary>
    /// 端口
    /// </summary>
    public uint Port { get; set; } = default!;

    /// <summary>
    /// 数据库
    /// </summary>
    public string Database { get; set; } = default!;

    /// <summary>
    /// 超时时间
    /// </summary>
    public int ReadWriteTimeout { get; set; } = default!;
    
    /// <summary>
    /// 
    /// </summary>
    public int LoadWeight { get; set; }
}