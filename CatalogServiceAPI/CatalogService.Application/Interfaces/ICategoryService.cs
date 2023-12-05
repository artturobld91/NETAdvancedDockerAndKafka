using CatalogService.Application.Dtos;
using CatalogService.Domain.Models;

namespace CatalogService.Application.Interfaces
{
    public interface ICategoryService
    {
        public IEnumerable<Category> GetCategories();

        public Category GetCategory(int id);

        public void AddCategory(CategoryCreateDto category);

        public void DeleteCategory(int id);

        public void UpdateCategory(CategoryUpdateDto category);
    }
}
