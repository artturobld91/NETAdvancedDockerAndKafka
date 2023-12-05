using System.ComponentModel.DataAnnotations;

namespace CartingService.BLL.Dtos
{
    public class CreateCartDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
