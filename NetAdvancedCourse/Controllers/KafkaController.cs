using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using NetAdvancedCourse.Dtos;
using System.Text.Json;

namespace NetAdvancedCourse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KafkaController : ControllerBase
    {
        private readonly ILogger<KafkaController> _logger;
        private readonly IConfiguration _config;

        public KafkaController(ILogger<KafkaController> logger,
                               IConfiguration config)
        {
            _logger = logger;
            _config = config;

        }

        [HttpGet("GetConfigs", Name = "GetConfigs")]
        public IActionResult Get()
        {
            var settings = _config.GetSection("Kafka").Get<Settings.Kafka>();
            return Ok( new { KafkaCluster = settings.BootstrapServers });
        }

        [HttpPost("Produce", Name = "Produce")]
        public async Task<IActionResult> Produce([FromBody] MessageDto message)
        {
            var settings = _config.GetSection("Kafka").Get<Settings.Kafka>();
            var config = new ProducerConfig
            {
                BootstrapServers = settings.BootstrapServers
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                var result = await producer.ProduceAsync("testtopic", new Message<Null, string> { Value = $"{message} {DateTime.UtcNow}" });
                return Ok(result);
            }
        }

        [HttpGet("Consume", Name = "Consume")]
        public IActionResult Consume()
        {
            var settings = _config.GetSection("Kafka").Get<Settings.Kafka>();
            var config = new ConsumerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                GroupId = "foo"
            };

            ConsumeResult<Ignore, string> consumeResult = null;

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(new List<string>() { "testtopic" });
                consumeResult = consumer.Consume();
                consumer.Close();
            }

            return consumeResult is not null ? Ok(consumeResult) : BadRequest("Failed to consume testtopic topic");
        }

        [HttpPost("ItemUpdated", Name = "ItemUpdated")]
        public async Task<IActionResult> ItemUpdated([FromBody] ItemUpdatedEventDto itemUpdatedEventDto)
        {
            var settings = _config.GetSection("Kafka").Get<Settings.Kafka>();
            var config = new ProducerConfig
            {
                BootstrapServers = settings.BootstrapServers
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var result = await producer.ProduceAsync("ItemUpdatedTopic", new Message<Null, string> { Value = JsonSerializer.Serialize(itemUpdatedEventDto) });
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest("Failed to send ItemUpdatedEventDto event to Kafka");
                }
            }
        }

        [HttpGet("GetItemUpdatedEvents", Name = "GetItemUpdatedEvents")]
        public IActionResult GetItemUpdatedEvents()
        {
            var settings = _config.GetSection("Kafka").Get<Settings.Kafka>();
            var config = new ConsumerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                GroupId = "foo"
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(new List<string>() { "ItemUpdatedTopic" });

                var consumeResult = consumer.Consume();
                consumer.Close();
                return Ok(consumeResult);
            }
        }
    }
}
