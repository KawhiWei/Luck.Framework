using System.Text.Json.Serialization;
using Luck.Framework.Infrastructure;

namespace Luck.Framework.Event
{
    /// <summary>
    /// 
    /// </summary>
    public class IntegrationEvent: IIntegrationEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public IntegrationEvent()
        {
            EventId = SnowflakeId.GenerateNewStringId();
            EventCreationDate = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonInclude] 
        public string EventId { get; } 
        /// <summary>
        /// 
        /// </summary>
        [JsonInclude]
        public DateTime EventCreationDate { get; }
    }


}
