using System.ComponentModel.DataAnnotations;

namespace CatalogService.Application.Events
{
    public class ItemUpdatedEventDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public int CategoryId { get; set; }
    }
}
