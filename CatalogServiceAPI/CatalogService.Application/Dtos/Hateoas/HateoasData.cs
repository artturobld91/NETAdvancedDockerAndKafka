using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Dtos.Hateoas
{
    public class HateoasData
    {
        public string Link { get; set; }
        public string Description { get; set; }
        public string Method { get; set; }
    }
}
