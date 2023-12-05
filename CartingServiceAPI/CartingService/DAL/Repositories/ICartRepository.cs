using CartingService.BLL.Dtos;

namespace CartingService.DAL.Repositories
{
    public interface ICartRepository : IDisposable
    {
        public Task CreateCart(CreateCartDto cart);
        public Task<List<ItemDto>> GetCartItems(Guid id);
        public Task<List<ItemDto>> GetItems();

        public Task AddItem(AddItemDto item);

        public Task RemoveItem(RemoveItemDto item);
        public Task UpdateItem(ItemDto item);
    }
}
