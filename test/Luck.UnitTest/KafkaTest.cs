using Confluent.Kafka;
using System.Threading.Tasks;
using Xunit;

namespace Luck.UnitTest
{
    public class KafkaTest
    {


        [Fact]
        public async Task Kafka_Connect_Test()
        {
            var config = new ProducerConfig { BootstrapServers = "192.168.31.11:30115",Acks =Acks.All };
            try
            {
                using (var p = new ProducerBuilder<Null, string>(config).Build())
                {
                    try
                    {
                        var dr = await p.ProduceAsync("test-topic", new Message<Null, string> { Value = "test" });
                        var aa = dr.Key;
                    }
                    catch (ProduceException<Null, string> e)
                    {

                    }
                }

            }
            catch (System.Exception)
            {

                throw;
            }

        }
    }
}
