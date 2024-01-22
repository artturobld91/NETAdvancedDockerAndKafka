using CartingService.BLL.Application;
using CartingService.BLL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.Controllers.V2
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private ICartService _cartService;

        public CartController(ILogger<CartController> logger,
                              ICartService cartService)
        {
            _logger = logger;
            _cartService = cartService;
        }

        [HttpPost("CreateCart")]
        [Authorize(Policy = "Manager")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCart()
        {
            CreateCartDto item = new CreateCartDto();
            item.Id = Guid.NewGuid();
            await _cartService.CreateCart(item);
            return Ok(item);
        }

        [HttpGet("GetCart/{id}")]
        [Authorize(Policy = "Manager, Buyer")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(IEnumerable<CartDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<ItemDto>> GetItems([FromRoute] Guid id)
        {
            return await _cartService.GetCartItems(id);
        }

        [HttpGet("GetItems")]
        [Authorize(Policy = "Manager, Buyer")]
        [ProducesResponseType(typeof(IEnumerable<ItemDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<ItemDto>> GetItems()
        {
            return await _cartService.GetItemListFromCart();
        }

        [HttpPost("AddItem")]
        [Authorize(Policy = "Manager")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddItem([FromBody] AddItemDto item)
        {
            item.Id = Guid.NewGuid();
            await _cartService.AddItemToCart(item);
            return Ok(item);
        }

        [HttpDelete("RemoveItem/{id}")]
        [Authorize(Policy = "Manager")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveItem([FromRoute] Guid id)
        {
            RemoveItemDto item = new RemoveItemDto();
            item.Id = id;
            await _cartService.RemoveItemFromCart(item);
            return NoContent();
        }
    }
}
