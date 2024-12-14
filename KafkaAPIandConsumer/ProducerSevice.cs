using Confluent.Kafka;

namespace KafkaProducerAPI
{
    public class ProducerSevice
    {
        private ILogger<ProducerSevice> _logger;

        public ProducerSevice(ILogger<ProducerSevice> logger)
        {
            _logger = logger;
        }

        public async Task ProduceAsync(CancellationToken cancellationToken)
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092",
                AllowAutoCreateTopics = true,
                Acks = Acks.All
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            try
            {
                var deliveryResult = await producer.ProduceAsync(topic: "test-topic",
                    new Message<Null, string> { 
                        Value = $"Hello Kafka!! {DateTime.UtcNow}" 
                    },
                    cancellationToken);

                _logger.LogInformation($"Deivered Message to {deliveryResult.Value}, Offset: {deliveryResult.Offset}");

            }
            catch (ProduceException<Null,string>e)
            {
                _logger.LogInformation($"Delivery failed {e.Error.Reason}");
            }

            producer.Flush(cancellationToken);

        }

    }
}
