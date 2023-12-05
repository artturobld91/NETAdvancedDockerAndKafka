using Confluent.Kafka;

namespace NetAdvancedCourse.Workers
{
    public sealed class KafkaConsumerWorker : BackgroundService
    {
        private readonly ILogger<KafkaConsumerWorker> _logger;
        private readonly IConfiguration _configuration;

        public KafkaConsumerWorker(ILogger<KafkaConsumerWorker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("KafkaConsumerWorker running at: {time}", DateTimeOffset.Now);

                var settings = _configuration.GetSection("Kafka").Get<Settings.Kafka>();
                var config = new ConsumerConfig
                {
                    BootstrapServers = settings.BootstrapServers,
                    GroupId = "foo"
                };

                using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumer.Subscribe(new List<string>() { "testtopic" });

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        _logger.LogInformation("KafkaConsumerWorker started consuming at: {time}", DateTimeOffset.Now);
                        var consumeResult = consumer.Consume();
                        _logger.LogInformation($"Kafka Message: {consumeResult.Message.Value} - Topic: {consumeResult.Topic}");
                        _logger.LogInformation("KafkaConsumerWorker ended consuming at: {time}", DateTimeOffset.Now);
                    }
                    
                    consumer.Close();
                }

                await Task.Delay(1_000, stoppingToken);
            }
        }
    }
}
