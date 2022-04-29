namespace Luck.Walnut.Dto.Applications
{
    public class ApplicationInputDto
    {
        /// <summary>
        /// 应用服务名称
        /// </summary>
        public string EnglishName { get; private set; } = default!;

        /// <summary>
        /// 应用所属部门
        /// </summary>
        public string DepartmentName { get; private set; } = default!;

        /// <summary>
        /// 应用中文名称
        /// </summary>
        public string ChinessName { get; private set; } = default!;

        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan { get; private set; } = default!;

        /// <summary>
        /// 应用唯一标识
        /// </summary>
        public string AppId { get; private set; } = default!;

        /// <summary>
        /// 应用状态
        /// </summary>
        public string Status { get; private set; } = default!;
    }
}
