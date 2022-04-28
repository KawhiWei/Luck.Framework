namespace Luck.Walnut.Dto.Environments
{
    public class AppEnvironmentInputDto
    {
        /// <summary>
        /// 环境名称
        /// </summary>
        public string EnvironmentName { get; set; } = default!;

        /// <summary>
        /// 应用Id
        /// </summary>
        public string ApplicationId { get; set; } = default!;
    }
}
