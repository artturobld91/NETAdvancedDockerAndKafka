using CartingService.BLL.Dtos;

namespace CartingService.BLL.Application
{
    public interface IItemService
    {
        public Task<IList<ItemDto>> GetItems(Guid itemCatalogId);
    }
}
