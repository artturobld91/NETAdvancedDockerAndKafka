using CatalogService.Application.Dtos;
using CatalogService.Application.Interfaces;
using CatalogService.Application.Mappers;
using CatalogService.Domain.Models;
using CatalogService.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Services
{
    public class ItemService : IItemService
    {
        private ApplicationDbContext _context;

        public ItemService()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<Item> GetItems()
        {
            return _context.Items.ToList();
        }

        public IEnumerable<Item> GetItemsPagination(PaginationDto paginationDto)
        {
            return _context.Items.Skip((paginationDto.Page - 1) * paginationDto.ItemsPerPage).Take(paginationDto.ItemsPerPage);
        }
        public IEnumerable<Item> GetItemsByCategoryId(int id)
        {
            return _context.Items.ToList().FindAll(item => item.CategoryId == id);
        }

        public Item GetItem(Guid id)
        {
            return _context.Items.First(item => item.Id == id);
        }

        public void AddItem(Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
        }

        public void DeleteItem(Guid id)
        {
            Item itemToDelete = _context.Items.FirstOrDefault(item => item.Id == id);
            _context.Items.Remove(itemToDelete);
            _context.SaveChanges();
        }

        public void UpdateItem(ItemUpdateDto item)
        {
            _context.Items.Update(item.ToModel());
            _context.SaveChanges();
        }
    }
}
