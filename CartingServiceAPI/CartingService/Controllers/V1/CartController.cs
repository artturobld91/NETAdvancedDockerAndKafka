using CartingService.BLL.Application;
using CartingService.BLL.Dtos;
using CartingService.DAL.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.Controllers.V1
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private ICartService _cartService;
        private MongoUnitOfWork _mongoUnitOfWork;

        public CartController(ILogger<CartController> logger, 
                              ICartService cartService, 
                              MongoUnitOfWork mongoUnitOfWork)
        {
            _logger = logger;
            _cartService = cartService; 
            _mongoUnitOfWork = mongoUnitOfWork;
        }

        [HttpPost("CreateCart")]
        [Authorize(Policy = "Manager")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCart()
        {
            CreateCartDto item = new CreateCartDto();
            item.Id = Guid.NewGuid();
            await _cartService.CreateCart(item);
            return Ok(item);
        }

        [HttpGet("GetCart/{id}")]
        [Authorize(Policy = "Manager")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
        public async Task<CartDto> GetItems([FromRoute] Guid id)
        {
            CartDto cart = new CartDto();
            cart.Id = id;
            cart.items = await _cartService.GetCartItems(id);
            return cart;
        }

        [HttpGet("GetItems")]
        [Authorize(Policy = "Manager")]
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
