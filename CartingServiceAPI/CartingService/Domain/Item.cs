using System.ComponentModel.DataAnnotations;

namespace CartingService.Domain
{
    public class Item
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Money { get; set; }
        [Required]
        public int Quantity { get; set; }
        public Uri Image { get; set; }
    }
}
