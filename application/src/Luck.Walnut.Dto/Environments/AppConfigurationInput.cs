﻿namespace Luck.Walnut.Dto.Environments
{
    public class AppConfigurationInput
    {
        /// <summary>
        /// 配置项Key
        /// </summary>
        public string Key { get;  set; } = default!;

        /// <summary>
        /// 配置项Value
        /// </summary>
        public string Value { get;  set; } = default!;

        /// <summary>
        /// 配置项类型
        /// </summary>
        public string Type { get;  set; } = default!;

        /// <summary>
        /// 是否公开(其他应用是否可获取)
        /// </summary>
        public bool IsOpen { get;  set; } = default!;

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublish { get;  set; } = default!;

    }


    public class UpdateAppConfigurationInputDto: AppConfigurationInput
    {
      public string Id { get; set; }

      public string EnvironmentId { get; set; }

    }
}
