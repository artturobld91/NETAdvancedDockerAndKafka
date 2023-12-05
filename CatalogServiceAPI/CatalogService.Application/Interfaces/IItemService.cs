using CatalogService.Application.Dtos;
using CatalogService.Domain.Models;

namespace CatalogService.Application.Interfaces
{
    public interface IItemService
    {
        public IEnumerable<Item> GetItems();

        public IEnumerable<Item> GetItemsPagination(PaginationDto paginationDto);

        public IEnumerable<Item> GetItemsByCategoryId(int id);

        public Item GetItem(Guid id);

        public void AddItem(Item item);

        public void DeleteItem(Guid id);

        public void UpdateItem(ItemUpdateDto item);
    }
}
