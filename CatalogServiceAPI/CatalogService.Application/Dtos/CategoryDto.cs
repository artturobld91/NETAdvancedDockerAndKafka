using CatalogService.Application.Dtos.Hateoas;
using CatalogService.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Dtos
{
    public class CategoryDto : HateoasLink
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<Item> Items { get; set; } = new();
    }
}
