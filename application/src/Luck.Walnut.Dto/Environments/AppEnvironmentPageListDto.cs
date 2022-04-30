namespace Luck.Walnut.Dto.Environments
{
    /// <summary>
    /// 分页集合输出DTO
    /// </summary>
    public class AppEnvironmentPageListOutputDto : AppEnvironmentBaseDto
    {


        public AppEnvironmentPageListOutputDto()
        {
            AppConfigurationOutputDtos = new List<AppConfigurationOutputDto>();
        }
        /// <summary>
        /// 环境名称
        /// </summary>
        public string EnvironmentName { get; set; } = default!;

        /// <summary>
        /// 应用Id
        /// </summary>
        public string ApplicationId { get; set; } = default!;


        public IEnumerable<AppConfigurationOutputDto> AppConfigurationOutputDtos { get; set; }
    }

    public class AppConfigurationOutputDto
    {

        public string Id { get; set; }

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
        /// 是否发布
        /// </summary>
        public bool IsPublish { get; set; } = default!;

    }
}
