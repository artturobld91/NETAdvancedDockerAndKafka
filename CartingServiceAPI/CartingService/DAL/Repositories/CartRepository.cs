using CartingService.BLL.Dtos;
using CartingService.Domain;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CartingService.DAL.Repositories
{
    public class CartRepository : ICartRepository, IDisposable
    {
        private MongoClient _client;
        public CartRepository(MongoClient client)
        {
            _client = client;
        }

        public async Task CreateCart(CreateCartDto cart)
        {
            var database = _client.GetDatabase("carting");
            var cartsCollection = database.GetCollection<CreateCartDto>("cart");
            await cartsCollection.InsertOneAsync(cart);
        }

        public CartDto GetCart(Guid id)
        {
            var database = _client.GetDatabase("carting");
            var cartsCollection = database.GetCollection<CartDto>("cart");
            return cartsCollection.Find(item => item.Id == id).FirstOrDefault();
        }

        public async Task<List<ItemDto>> GetItems()
        {
            var database = _client.GetDatabase("carting");
            var itemsCollection = database.GetCollection<ItemDto>("items");
            return await itemsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<List<ItemDto>> GetCartItems(Guid id)
        {
            var database = _client.GetDatabase("carting");
            var itemsCollection = database.GetCollection<ItemDto>("items");
            return await itemsCollection.Find(item => item.CartId == id).ToListAsync();
        }

        public async Task AddItem(AddItemDto item)
        {
            var database = _client.GetDatabase("carting");
            var itemsCollection = database.GetCollection<AddItemDto>("items");
            await itemsCollection.InsertOneAsync(item);
        }

        public async Task RemoveItem(RemoveItemDto item)
        {
            var database = _client.GetDatabase("carting");
            var itemsCollection = database.GetCollection<Item>("items");
            await itemsCollection.DeleteOneAsync(itemCol => itemCol.Id == item.Id);
        }

        public async Task UpdateItem(ItemDto item)
        {
            var database = _client.GetDatabase("carting");
            var itemsCollection = database.GetCollection<ItemDto>("items");
            var filter = Builders<ItemDto>.Filter.Eq(s => s.Id, item.Id);
            await itemsCollection.ReplaceOneAsync(filter, item);
        }

        private bool disposed = false;

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
