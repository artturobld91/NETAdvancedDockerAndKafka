using CatalogService.Application.Dtos;
using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Domain.Models;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Application.GraphQLQueries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class ItemQueries
    {
        private IItemService _itemService;

        public ItemQueries()
        {
            _itemService = new ItemService();
        }

        public IEnumerable<Item> GetItems()
        {
            return _itemService.GetItems();
        }
        public IEnumerable<Item> GetItemsByCategoryId(int id)
        {
            return _itemService.GetItemsByCategoryId(id);
        }

        public IEnumerable<Item> GetItemsPagination(PaginationDto paginationDto)
        {
            return _itemService.GetItemsPagination(paginationDto);
        }
    }

    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class ItemMutations
    {
        private IItemService _itemService;

        public ItemMutations()
        {
            _itemService = new ItemService();
        }

        public Item AddItem(Item item)
        {
            _itemService.AddItem(item);
            return item;
        }

        public ItemUpdateDto UpdateItem(ItemUpdateDto item)
        {
            _itemService.UpdateItem(item);
            return item;
        }

        public Guid DeleteItem(Guid id)
        {
            _itemService.DeleteItem(id);
            return id;
        }
    }
}
