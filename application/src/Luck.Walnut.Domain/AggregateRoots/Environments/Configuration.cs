using Luck.DDD.Domain;

namespace Luck.Walnut.Domain.AggregateRoots.Environments
{
    public class Configuration : FullEntity
    { 
    
        /// <summary>
        /// 配置项Key
        /// </summary>
        public string Key { get; private set; } = default!;

        /// <summary>
        /// 配置项Value
        /// </summary>
        public string Value { get; private set; } = default!;

        /// <summary>
        /// 配置项类型
        /// </summary>
        public string Type { get; private set; } = default!;

        /// <summary>
        /// 是否公开(其他应用是否可获取)
        /// </summary>
        public bool IsOpen { get; private set; } = default!;

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublish { get; private set; } = default!;

    }

}
