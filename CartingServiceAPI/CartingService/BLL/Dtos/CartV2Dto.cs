using System.ComponentModel.DataAnnotations;

namespace CartingService.BLL.Dtos
{
    public class CartV2Dto
    {
        [Required]
        public string Id { get; set; }
    }
}
