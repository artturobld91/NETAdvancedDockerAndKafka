using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace NetAdvancedCourse.Workers
{
    public class KafkaConsumerHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private Timer? _timer = null;
        private readonly ILogger<KafkaConsumerHostedService> _logger;
        private readonly IConfiguration _configuration;

        public KafkaConsumerHostedService(ILogger<KafkaConsumerHostedService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
            "KafkaConsumerHostedService is working. Count: {Count}", count);

            var settings = _configuration.GetSection("Kafka").Get<Settings.Kafka>();
            var config = new ConsumerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                GroupId = "foo"
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(new List<string>() { "testtopic" });
                _logger.LogInformation("KafkaConsumerHostedService started consuming at: {time}", DateTimeOffset.Now);
                var consumeResult = consumer.Consume();
                _logger.LogInformation($"Kafka Message: {consumeResult.Message.Value} - Topic: {consumeResult.Topic}");
                _logger.LogInformation("KafkaConsumerHostedService ended consuming at: {time}", DateTimeOffset.Now);
                consumer.Close();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}
