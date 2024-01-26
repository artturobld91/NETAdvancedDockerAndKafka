using CatalogService.Application.Dtos;
using CatalogService.Application.Interfaces;
using CatalogService.Application.Mappers;
using CatalogService.Domain.Models;
using CatalogService.Infrastructure.Database;

namespace CatalogService.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private ApplicationDbContext _context;

        public CategoryService()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(category => category.Id == id);
        }

        public void AddCategory(CategoryCreateDto category)
        {
            _context.Categories.Add(category.ToModel());
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            Category categoryToDelete = _context.Categories.FirstOrDefault(category => category.Id == id);
            List<Item> relatedItems = _context.Items.Where(item => item.CategoryId == id).ToList();
            if (relatedItems.Count > 0)
            {
                relatedItems.ForEach(relatedItem => _context.Items.Remove(relatedItem));
            }
            _context.Categories.Remove(categoryToDelete);
            _context.SaveChanges();
        }

        public void UpdateCategory(CategoryUpdateDto category)
        {
            _context.ChangeTracker.Clear();
            _context.Categories.Update(category.ToModel());
            _context.SaveChanges();
        }
    }
}
