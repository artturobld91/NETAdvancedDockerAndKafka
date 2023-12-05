using CartingService.BLL.Dtos;
using System.Runtime.CompilerServices;

namespace CartingService.BLL.Mappers
{
    public static class ItemUpdatedEventDtoMapper
    {
        public static ItemDto ToItemDto(this ItemUpdatedEventDto itemUpdatedEventDto)
        { 
            var itemDto = new ItemDto();
            itemDto.Id = itemUpdatedEventDto.Id;
            itemDto.Name = itemUpdatedEventDto.Name;
            itemDto.Money = itemUpdatedEventDto.Amount;
            return itemDto;
        }
    }
}
