using CatalogService.Application.Dtos;
using CatalogService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Mappers
{
    public static class CategoryDtoMapper
    {
        public static CategoryDto ToDto(this Category category)
        {
            if (category is null)
                return null;

            CategoryDto dto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
                Url = category.Url,
                Items = category.Items
            };

            return dto;
        }

        public static IEnumerable<CategoryDto> ToDto(this IEnumerable<Category> categories)
        {
            List<CategoryDto> dtoList = new List<CategoryDto>();

            foreach (Category category in categories)
            {
                CategoryDto dto = new CategoryDto()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Url = category.Url,
                    Items = category.Items
                };
                dtoList.Add(dto);
            }
            return dtoList;
        }

        public static Category ToModel(this CategoryDto dto)
        {
            Category category = new Category()
            {
                Id = dto.Id,
                Name = dto.Name,
                Url = dto.Url,
                Items = dto.Items
            };
            return category;
        }

        public static Category ToModel(this CategoryCreateDto dto)
        {
            Category category = new Category()
            {
                Id = dto.Id,
                Name = dto.Name,
                Url = dto.Url,
                Items = new List<Item>()
            };
            return category;
        }

        public static Category ToModel(this CategoryUpdateDto dto)
        {
            Category category = new Category()
            {
                Id = dto.Id,
                Name = dto.Name,
                Url = dto.Url,
                Items = new List<Item>()
            };
            return category;
        }

        public static List<Category> ToModel(this List<CategoryDto> dtos)
        {
            List<Category> categoryList = new List<Category>();

            foreach (CategoryDto dto in dtos)
            {
                Category category = new Category()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Url = dto.Url,
                    Items = dto.Items
                };
                categoryList.Add(category);
            }
            return categoryList;
        }
    }
}
