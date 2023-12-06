using CartingService.BLL.Dtos;
using CartingService.DAL.Database;

namespace CartingService.BLL.Application
{
    public class ItemService : IItemService
    {
        private MongoUnitOfWork _mongo;
        public ItemService(MongoUnitOfWork mongo)
        {
            _mongo = mongo;
        }

        public async Task<IList<ItemDto>> GetItems(Guid itemCatalogId)
        {
            return await _mongo._itemRepository.GetItems(itemCatalogId);
        }
    }
}
