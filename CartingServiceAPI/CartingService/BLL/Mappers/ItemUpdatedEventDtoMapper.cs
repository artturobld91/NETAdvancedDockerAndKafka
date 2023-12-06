using CartingService.BLL.Dtos;

namespace CartingService.BLL.Mappers
{
    public static class ItemUpdatedEventDtoMapper
    {
        public static ItemDto ToItemDto(this ItemUpdatedEventDto itemUpdatedEventDto)
        { 
            var itemDto = new ItemDto();
            itemDto.ItemCatalogId = itemUpdatedEventDto.Id;
            itemDto.Name = itemUpdatedEventDto.Name;
            itemDto.Money = itemUpdatedEventDto.Amount;
            itemDto.Image = itemUpdatedEventDto.Image;
            return itemDto;
        }
    }
}
