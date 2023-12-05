using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Dtos
{
    public class PaginationDto
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; } = 0;
        public int PageCount { get; set; }
    }
}
