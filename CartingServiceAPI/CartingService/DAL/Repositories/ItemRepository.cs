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
