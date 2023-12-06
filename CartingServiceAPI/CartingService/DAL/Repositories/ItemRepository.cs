using CartingService.BLL.Dtos;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CartingService.DAL.Repositories
{
    public class ItemRepository : IItemRepository, IDisposable
    {
        private MongoClient _client;
        public ItemRepository(MongoClient client)
        {
            _client = client;
        }

        private bool disposed = false;

        public async Task<List<ItemDto>> GetItems(Guid itemCatalogId)
        {
            var database = _client.GetDatabase("carting");
            var itemsCollection = database.GetCollection<ItemDto>("items");
            var filter = Builders<ItemDto>.Filter.Eq(s => s.ItemCatalogId, itemCatalogId);
            return await itemsCollection.Find(filter).ToListAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // No need to dispose MongoClient
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
