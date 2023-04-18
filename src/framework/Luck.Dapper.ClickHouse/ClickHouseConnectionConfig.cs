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