using Luck.DDD.Domain;

namespace Luck.Walnut.Domain.AggregateRoots.Environments
{
    
    public class AppConfiguration : FullEntity
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

        /// <summary>
        /// 环境Id
        /// </summary>
        public string AppEnvironmentId { get; private set; } = default!;

        private AppConfiguration()
        {

        }

        public AppConfiguration(string key, string value, string type,bool isOpen) :this()
        {
            Key = key;
            Value = value;
            Type = type;
            IsOpen = isOpen;
        }

        public AppConfiguration UpdateConfiguration(string key, string value, string type, bool isOpen)
        {
            Key = key;
            Value = value;
            Type = type;
            IsOpen = isOpen;
            IsPublish = false;
            return this;
        }

        public AppConfiguration ChangeKey(string key)
        {

            this.Key = key;
            return this;
        }

        public AppConfiguration ChangeValue(string value)
        {

            this.Value = value;
            return this;
        }


        public AppConfiguration ChangeType(string type)
        {

            this.Type = type;
            return this;
        }

        /// <summary>
        /// 更改开启
        /// </summary>
        /// <param name="isOpen"></param>
        public AppConfiguration ChangeOpen(bool isOpen)
        {


            this.IsOpen = isOpen;
            return this;
        }

        /// <summary>
        /// 更改发布
        /// </summary>
        /// <param name="isPublish"></param>

        public AppConfiguration ChangePublish(bool isPublish)
        {

            this.IsPublish = isPublish;
            return this;
        }

    }

}
