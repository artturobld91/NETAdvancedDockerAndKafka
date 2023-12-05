using CatalogService.Application.Dtos;
using CatalogService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Mappers
{
    public static class ItemDtoMapper
    {
        public static ItemDto ToDto(this Item item)
        {
            ItemDto dto = new ItemDto()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Image = item.Image,
                Price = item.Price,
                Amount = item.Amount,
                CategoryId = item.CategoryId,
            };
            return dto;
        }

        public static IEnumerable<ItemDto> ToDto(this IEnumerable<Item> items)
        {
            List<ItemDto> dtos = new List<ItemDto>();

            foreach (Item item in items)
            {
                ItemDto dto = new ItemDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Image = item.Image,
                    Price = item.Price,
                    Amount = item.Amount,
                    CategoryId = item.CategoryId,
                };
                dtos.Add(dto);
            }
            return dtos;
        }

        public static Item ToModel(this ItemDto dto)
        {
            Item item = new Item()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image,
                Price = dto.Price,
                Amount = dto.Amount,
                CategoryId = dto.CategoryId,
            };
            return item;
        }

        public static Item ToModel(this ItemCreateDto dto)
        {
            Item item = new Item()
            {
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image,
                Price = dto.Price,
                Amount = dto.Amount,
                CategoryId = dto.CategoryId,
            };
            return item;
        }

        public static Item ToModel(this ItemUpdateDto dto)
        {
            Item item = new Item()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image,
                Price = dto.Price,
                Amount = dto.Amount,
                CategoryId = dto.CategoryId,
            };
            return item;
        }

        public static List<Item> ToModel(this List<ItemDto> dtos)
        {
            List<Item> items = new List<Item>();

            foreach (ItemDto dto in dtos)
            {
                Item item = new Item()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Description = dto.Description,
                    Image = dto.Image,
                    Price = dto.Price,
                    Amount = dto.Amount,
                    CategoryId = dto.CategoryId,
                };
                items.Add(item);
            }
            return items;
        }
    }
}
