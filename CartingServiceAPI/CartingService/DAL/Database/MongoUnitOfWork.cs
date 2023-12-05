using CartingService.DAL.Repositories;
using MongoDB.Driver;

namespace CartingService.DAL.Database
{
    public class MongoUnitOfWork : IDisposable
    {
        private MongoClient _client;
        public ICartRepository _cartRepository;
        public IItemRepository _itemRepository;
        public MongoUnitOfWork()
        {
            _client = MongoDBConnection.GetConnection();
            _cartRepository = new CartRepository(_client);
            _itemRepository = new ItemRepository(_client);
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
