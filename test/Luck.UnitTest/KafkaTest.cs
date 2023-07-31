using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
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
                        var dr = await p.ProduceAsync("test-topic-asdadasdasdas", new Message<Null, string> { Value = "test" });
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

        [Fact]
        public async Task Test_Consumer()
        {


            var conf = new ConsumerConfig
            {
                GroupId = "csharp-consumer",
                BootstrapServers = "192.168.31.90:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
            };
            using (var consumer = new ConsumerBuilder<Ignore, string>(conf)
                // Note: All handlers are called on the main .Consume thread.
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .SetStatisticsHandler((_, json) => Console.WriteLine($"Statistics: {json}"))
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    // Since a cooperative assignor (CooperativeSticky) has been configured, the
                    // partition assignment is incremental (adds partitions to any existing assignment).


                    // Possibly manually specify start offsets by returning a list of topic/partition/offsets
                    // to assign to, e.g.:
                    // return partitions.Select(tp => new TopicPartitionOffset(tp, externalOffsets[tp]));
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    // Since a cooperative assignor (CooperativeSticky) has been configured, the revoked
                    // assignment is incremental (may remove only some partitions of the current assignment).
                    var remaining = c.Assignment.Where(atp => partitions.Where(rtp => rtp.TopicPartition == atp).Count() == 0);

                })
                .SetPartitionsLostHandler((c, partitions) =>
                {
                    // The lost partitions handler is called when the consumer detects that it has lost ownership
                    // of its assignment (fallen out of the group).

                })
                .Build())
            {
                consumer.Subscribe("test-topic");

                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume();

                            if (consumeResult.IsPartitionEOF)
                            {


                                continue;
                            }


                            try
                            {
                                // Store the offset associated with consumeResult to a local cache. Stored offsets are committed to Kafka by a background thread every AutoCommitIntervalMs. 
                                // The offset stored is actually the offset of the consumeResult + 1 since by convention, committed offsets specify the next message to consume. 
                                // If EnableAutoOffsetStore had been set to the default value true, the .NET client would automatically store offsets immediately prior to delivering messages to the application. 
                                // Explicitly storing offsets after processing gives at-least once semantics, the default behavior does not.
                                consumer.StoreOffset(consumeResult);
                            }
                            catch (KafkaException e)
                            {

                            }
                        }
                        catch (ConsumeException e)
                        {

                        }
                    }
                }
                catch (OperationCanceledException)
                {

                    consumer.Close();
                }
            }
        }


    }
}

