using CatalogService.Application.Dtos;
using CatalogService.Application.Events;

namespace CatalogService.Application.Mappers
{
    public static class ItemUpdatedEventDtoMapper
    {
        public static ItemUpdatedEventDto ToItemUpdatedEventDto(this ItemUpdateDto item)
        { 
            ItemUpdatedEventDto eventDto = new ItemUpdatedEventDto();
            eventDto.Id = item.Id;
            eventDto.Name = item.Name;
            eventDto.Description = item.Description;
            eventDto.Image = item.Image;
            eventDto.Price = item.Price;
            eventDto.Amount = item.Amount;
            eventDto.CategoryId = item.CategoryId;

            return eventDto;
        }
    }
}
