using System.Text.Json.Serialization;

namespace Luck.Framework.Event
{
    public class IntegrationEvent: IIntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = SnowflakeId.GenerateNewStringId();
            CreationDate = DateTime.Now;
        }
        [JsonInclude]
        public string Id { get; }
        [JsonInclude]
        public DateTime CreationDate { get; }
    }


}
