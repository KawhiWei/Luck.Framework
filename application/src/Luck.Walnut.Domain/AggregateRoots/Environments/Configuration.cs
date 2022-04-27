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


        private Configuration()
        {

        }


        ///key  - value -type 要不要使用值对象  ？？
        public static Configuration Create(string key, string value, string type)
        {


            Configuration configuration = new();
            configuration.Key = key;
            configuration.Value = value;
            configuration.Type = type;
            return configuration;
        }


        public Configuration ChangeKey(string key)
        {

            this.Key = key;
            return this;
        }

        public Configuration ChangeValue(string value)
        {

            this.Value = value;
            return this;
        }


        public Configuration ChangeType(string type)
        {

            this.Type = type;
            return this;
        }

        /// <summary>
        /// 更改开启
        /// </summary>
        /// <param name="isOpen"></param>
        public Configuration ChangeOpen(bool isOpen)
        {


            this.IsOpen = isOpen;
            return this;
        }

        /// <summary>
        /// 更改发布
        /// </summary>
        /// <param name="isPublish"></param>

        public Configuration ChangePublish(bool isPublish)
        {

            this.IsPublish = isPublish;
            return this;
        }

    }

}
