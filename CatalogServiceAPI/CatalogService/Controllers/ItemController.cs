using CatalogService.Application;
using CatalogService.Application.Dtos;
using CatalogService.Application.Events;
using CatalogService.Application.Interfaces;
using CatalogService.Application.Mappers;
using CatalogService.Application.Services;
using CatalogService.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;
        private readonly IConfiguration _configuration;
        private IItemService _itemService;
        private IHttpClientFactory _httpClientFactory;

        public ItemController(ILogger<ItemController> logger, 
                              IHttpClientFactory httpClientFactory,
                              IConfiguration configuration)
        {
            _logger = logger;
            _itemService = new ItemService();
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet("GetItems")]
        [ServiceFilter(typeof(HateoasItemFilterAttribute))]
        [ProducesResponseType(typeof(IEnumerable<ItemDto>), StatusCodes.Status200OK)]
        public IEnumerable<ItemDto> GetItems()
        {
            return _itemService.GetItems().ToDto();
        }

        [HttpGet("GetItemsByCategoryId/{id}")]
        [ProducesResponseType(typeof(IEnumerable<ItemDto>), StatusCodes.Status200OK)]
        public IEnumerable<ItemDto> GetItemsByCategoryId([FromRoute] int id)
        {
            return _itemService.GetItemsByCategoryId(id).ToDto();
        }

        [HttpGet("GetItem/{id}", Name = "GetItem")]
        [ServiceFilter(typeof(HateoasItemFilterAttribute))]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
        public ItemDto GetItem([FromRoute] Guid id)
        {
            return _itemService.GetItem(id).ToDto();
        }

        [HttpPost("GetItemsPagination")]
        [ProducesResponseType(typeof(IEnumerable<ItemDto>), StatusCodes.Status200OK)]
        public IEnumerable<ItemDto> GetItemsPagination([FromBody] PaginationDto paginationDto)
        {
            return _itemService.GetItemsPagination(paginationDto).ToDto();
        }

        [HttpPost("AddItem")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status201Created)]
        public IActionResult AddItem([FromBody] ItemCreateDto item)
        {
            _itemService.AddItem(item.ToModel());
            return CreatedAtAction(nameof(GetItem), item);
        }

        [HttpDelete("RemoveItem/{id}", Name = "RemoveItem")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status204NoContent)]
        public IActionResult RemoveItem([FromRoute] Guid id)
        {
            _itemService.DeleteItem(id);
            return NoContent();
        }

        [HttpPut("UpdateItem/{id}", Name = "UpdateItem")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateItem([FromRoute] Guid id, [FromBody] ItemUpdateDto item)
        {
            item.Id = id;
            _itemService.UpdateItem(item);

            // Calling Kafka Controller to send ItemUpdatedEvent
            string kafkaEndpointUrl = $"{_configuration["Kafka"]}/Kafka/ItemUpdated";
            Uri uri = new Uri(kafkaEndpointUrl);

            ItemUpdatedEventDto itemUpdatedEventDto = item.ToItemUpdatedEventDto();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            httpRequestMessage.Content = JsonContent.Create(itemUpdatedEventDto);
            HttpClient httpClient = _httpClientFactory.CreateClient();
            var httpsResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpsResponseMessage.IsSuccessStatusCode)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Failed to send ItemUpdatedEvent to kafka api.");
            }
        }
    }
}
