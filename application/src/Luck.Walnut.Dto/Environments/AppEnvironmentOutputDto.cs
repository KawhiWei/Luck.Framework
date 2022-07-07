namespace Luck.Walnut.Dto.Environments;

public class AppEnvironmentOutputDto
{
    /// <summary>
    /// 环境名称
    /// </summary>
    public string EnvironmentName { get;  set; } = default!;

    /// <summary>
    /// 应用Id
    /// </summary>
    public string AppId { get;  set; } = default!;
        
    /// <summary>
    /// 版本（每次修改配置时更新版本号）
    /// </summary>
    public string Version { get;  set; } = default!;
        
    public  List<AppConfigurationOutputDto> Configs { get; set; } = default!;
}