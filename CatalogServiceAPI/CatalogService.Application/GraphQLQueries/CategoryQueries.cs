using CatalogService.Application.Dtos;
using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Domain.Models;
using HotChocolate.Types;

namespace CatalogService.Application.GraphQLQueries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class CategoryQueries
    {
        private ICategoryService _categoryService;

        public CategoryQueries()
        {
            _categoryService = new CategoryService();
        }

        public IEnumerable<Category> GetCategories()
        {
            return _categoryService.GetCategories();
        }
    }

    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class CategoryMutations
    {
        private ICategoryService _categoryService;

        public CategoryMutations()
        {
            _categoryService = new CategoryService();
        }

        public CategoryCreateDto AddCategory(CategoryCreateDto category)
        {
            #region Example client usage
            /// <summary>
            /// Mutation example to Add Category
            /// </summary>

            //  mutation {
            //      addCategory(category: { id: 0, name: "Test Category", url: "http://TestCategory.com" }
            //      ) {
            //        id
            //        name
            //        url
            //      }
            //  }
            #endregion

            _categoryService.AddCategory(category);
            return category;
        }

        public CategoryUpdateDto UpdateCategory(CategoryUpdateDto category)
        {
            #region Example client usage
            /// <summary>
            /// Mutation example to Update Category
            /// </summary>

            //  mutation {
            //      updateCategory(category: { id: 0, name: "Test Category", url: "http://TestCategory.com" }
            //      ) {
            //        id
            //        name
            //        url
            //      }
            //  }
            #endregion

            _categoryService.UpdateCategory(category);
            return category;
        }

        public int DeleteCategory(int id)
        {
            _categoryService.DeleteCategory(id);
            return id;
        }
    }
}
