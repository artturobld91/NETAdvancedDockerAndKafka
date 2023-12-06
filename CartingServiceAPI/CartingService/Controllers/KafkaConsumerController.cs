using CartingService.BLL.Application;
using CartingService.BLL.Dtos;
using CartingService.BLL.Mappers;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace CartingService.Controllers
{
    [ApiController]
    public class KafkaConsumerController : ControllerBase
    {
        private readonly ILogger<KafkaConsumerController> _logger;
        private readonly IConfiguration _configuration;
        private ICartService _cartService;
        private IItemService _itemService;

        public KafkaConsumerController(ILogger<KafkaConsumerController> logger, 
                                        ICartService cartService,
                                        IItemService itemService,
                                        IConfiguration configuration)
        {
            _logger = logger;
            _cartService = cartService;
            _itemService = itemService;
            _configuration = configuration;
        }

        [HttpGet("GetEvents")]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEvents()
        {
            var settings = _configuration.GetSection("Kafka").Get<Settings.Kafka>();
            var config = new ConsumerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                GroupId = "ItemUpdatedConsumers"
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(new List<string>() { "ItemUpdatedTopic" });

                _logger.LogInformation("KafkaConsumerWorker started consuming at: {time}", DateTimeOffset.Now);
                var consumeResult = consumer.Consume();
                ItemUpdatedEventDto itemUpdated = JsonSerializer.Deserialize<ItemUpdatedEventDto>(consumeResult.Message.Value);
                _logger.LogInformation($"Kafka Message: {itemUpdated} - Topic: {consumeResult.Topic}");
                _logger.LogInformation($"Item to update: {itemUpdated.Id} - Topic: {consumeResult.Topic}");
                _logger.LogInformation($"Item description: {itemUpdated.Description} - Topic: {consumeResult.Topic}");

                // Update Cart items
                ItemDto item = itemUpdated.ToItemDto();
                List<ItemDto> items = (List<ItemDto>) await _itemService.GetItems(item.ItemCatalogId.Value);

                foreach (ItemDto tempItem in items)
                {
                    tempItem.Name = item.Name;
                    tempItem.Money = item.Money;
                    tempItem.Image = item.Image;

                    await _cartService.UpdateItemFromCart(tempItem);
                    _logger.LogInformation($"Item updated: {tempItem.Id} - ItemCatalogId: {tempItem.ItemCatalogId} - Topic: {consumeResult.Topic}");
                }

                _logger.LogInformation("KafkaConsumerWorker ended consuming at: {time}", DateTimeOffset.Now);

                consumer.Close();
            }
            return Ok();
        }
    }
}
