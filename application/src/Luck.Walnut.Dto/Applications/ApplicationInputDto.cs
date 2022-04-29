namespace Luck.Walnut.Dto.Applications
{
    public class ApplicationInputDto
    {
        /// <summary>
        /// 应用服务名称
        /// </summary>
        public string EnglishName { get; set; } = default!;

        /// <summary>
        /// 应用所属部门
        /// </summary>
        public string DepartmentName { get; set; } = default!;

        /// <summary>
        /// 应用中文名称
        /// </summary>
        public string ChinessName { get; set; } = default!;

        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan { get; set; } = default!;

        /// <summary>
        /// 应用唯一标识
        /// </summary>
        public string AppId { get; set; } = default!;

        /// <summary>
        /// 应用状态
        /// </summary>
        public string Status { get; set; } = default!;
    }

    public class ApplicationOutputDto
    {
        public string Id { get; set; } = default!;
        /// <summary>
        /// 应用服务名称
        /// </summary>
        public string EnglishName { get; set; } = default!;

        /// <summary>
        /// 应用所属部门
        /// </summary>
        public string DepartmentName { get; set; } = default!;

        /// <summary>
        /// 应用中文名称
        /// </summary>
        public string ChinessName { get; set; } = default!;

        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan { get; set; } = default!;

        /// <summary>
        /// 应用唯一标识
        /// </summary>
        public string AppId { get; set; } = default!;

        /// <summary>
        /// 应用状态
        /// </summary>
        public string Status { get; set; } = default!;
    }
}
