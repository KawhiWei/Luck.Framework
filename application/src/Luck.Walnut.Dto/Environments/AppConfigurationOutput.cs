namespace Luck.Walnut.Dto.Environments
{
    public class AppConfigurationOutput
    {
        public string Id { get; set; } = default!;

        /// <summary>
        /// 配置项Key
        /// </summary>
        public string Key { get; set; } = default!;

        /// <summary>
        /// 配置项Value
        /// </summary>
        public string Value { get; set; } = default!;

        /// <summary>
        /// 配置项类型
        /// </summary>
        public string Type { get; set; } = default!;

        /// <summary>
        /// 是否公开(其他应用是否可获取)
        /// </summary>
        public bool IsOpen { get; set; } = default!;

        /// <summary>
        /// 组
        /// </summary>
        public string Group { get; set; } = default!;

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublish { get; set; } = default!;

    }

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
    }

}
