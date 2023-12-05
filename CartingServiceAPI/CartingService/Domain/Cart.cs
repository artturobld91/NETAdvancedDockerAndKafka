using System.ComponentModel.DataAnnotations;

namespace CartingService.Domain
{
    public class Cart
    {
        [Required]
        public string Id { get; set; }
        public List<Item> items { get; set; }
    }
}
