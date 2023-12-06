using CartingService.BLL.Dtos;

namespace CartingService.DAL.Repositories
{
    public interface IItemRepository : IDisposable
    {
        public Task<List<ItemDto>> GetItems(Guid itemCatalogId);
    }
}
