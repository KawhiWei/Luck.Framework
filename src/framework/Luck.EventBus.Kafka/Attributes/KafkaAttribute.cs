using System.ComponentModel;
using Luck.Framework.Extensions;

namespace Luck.EventBus.Kafka.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class KafkaAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="tag">标签</param>
        public KafkaAttribute(string topic, string tag)
        {
            Topic = topic;
            Tag = tag;
        }

        /// <summary>
        /// 主题
        /// </summary>
        public string Topic { get; private set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; private set; }
        
    }
}