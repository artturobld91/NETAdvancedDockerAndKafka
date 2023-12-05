using CartingService.BLL.Dtos;

namespace CartingService.BLL.Application
{
    public interface ICartService
    {
        public Task CreateCart(CreateCartDto cart);
        public Task<List<ItemDto>> GetItemListFromCart();
        public Task<IList<ItemDto>> GetCartItems(Guid id);
        public Task AddItemToCart(AddItemDto item);
        public Task RemoveItemFromCart(RemoveItemDto item);
        public Task UpdateItemFromCart(ItemDto item);
    }
}
