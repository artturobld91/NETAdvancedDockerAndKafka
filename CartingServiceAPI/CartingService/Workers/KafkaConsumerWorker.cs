using CartingService.BLL.Application;
using CartingService.BLL.Dtos;
using CartingService.BLL.Mappers;
using Confluent.Kafka;
using System.Text.Json;

namespace CartingService.Workers
{
    public sealed class KafkaConsumerWorker : BackgroundService
    {
        private readonly ILogger<KafkaConsumerWorker> _logger;
        private readonly IConfiguration _configuration;
        private ICartService _cartService;

        public KafkaConsumerWorker(ILogger<KafkaConsumerWorker> logger, IConfiguration configuration, ICartService cartService)
        {
            _logger = logger;
            _configuration = configuration;
            _cartService = cartService;
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
                    GroupId = "ItemUpdatedConsumers"
                };

                using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumer.Subscribe(new List<string>() { "ItemUpdatedTopic" });

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        _logger.LogInformation("KafkaConsumerWorker started consuming at: {time}", DateTimeOffset.Now);
                        var consumeResult = consumer.Consume();
                        var itemUpdated = JsonSerializer.Deserialize<ItemUpdatedEventDto>(consumeResult.Message.Value);
                        _logger.LogInformation($"Kafka Message: {itemUpdated} - Topic: {consumeResult.Topic}");
                        _logger.LogInformation($"Item to update: {itemUpdated.Id} - Topic: {consumeResult.Topic}");
                        _logger.LogInformation($"Item description: {itemUpdated.Description} - Topic: {consumeResult.Topic}");

                        // Update Cart items
                        ItemDto item = itemUpdated.ToItemDto();
                        await _cartService.UpdateItemFromCart(item);
                        _logger.LogInformation($"Item updated: {itemUpdated.Id} - Topic: {consumeResult.Topic}");

                        _logger.LogInformation("KafkaConsumerWorker ended consuming at: {time}", DateTimeOffset.Now);
                    }

                    consumer.Close();
                }

                await Task.Delay(60_000, stoppingToken);
            }
        }
    }
}
