using System.ComponentModel.DataAnnotations;

namespace CartingService.BLL.Dtos
{
    public class CartDto
    {
        [Required]
        public Guid Id { get; set; }
        public IList<ItemDto> items { get; set; }
    }
}
