using System.Text.Json.Serialization;
using Luck.Framework.Infrastructure;

namespace Luck.Framework.Event
{
    public class IntegrationEvent: IIntegrationEvent
    {
        public IntegrationEvent()
        {
            EventId = SnowflakeId.GenerateNewStringId();
            EventCreationDate = DateTime.Now;
        }
        [JsonInclude]
        public string EventId { get; }
        [JsonInclude]
        public DateTime EventCreationDate { get; }
    }


}
