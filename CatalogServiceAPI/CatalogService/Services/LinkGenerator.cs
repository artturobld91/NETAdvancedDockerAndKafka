using CatalogService.Application.Dtos;
using CatalogService.Application.Dtos.Hateoas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace CatalogService.Services
{
    public class LinkGenerator
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _actionContextAccessor;

        public LinkGenerator(IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task GenerateCategoryLinks(CategoryDto dto)
        {
            var url = BuildUrlHelper();

            dto.Links.Add(
                new HateoasData() 
                { 
                    Link = url.Link("GetCategory", new { id = dto.Id }), 
                    Description = "self", 
                    Method = "GET" 
                });

            dto.Links.Add(
                new HateoasData()
                {
                    Link = url.Link("UpdateCategory", new { id = dto.Id }),
                    Description = "self",
                    Method = "PUT"
                });

            dto.Links.Add(
                new HateoasData()
                {
                    Link = url.Link("DeleteCategory", new { id = dto.Id }),
                    Description = "self",
                    Method = "DELETE"
                });
        }

        public async Task GenerateCategoryLinks(IEnumerable<CategoryDto> dtos)
        {
            var url = BuildUrlHelper();

            foreach (CategoryDto dto in dtos)
            {
                dto.Links.Add(
                    new HateoasData()
                    {
                        Link = url.Link("GetCategory", new { id = dto.Id }),
                        Description = "self",
                        Method = "GET"
                    });

                dto.Links.Add(
                    new HateoasData()
                    {
                        Link = url.Link("UpdateCategory", new { id = dto.Id }),
                        Description = "self",
                        Method = "PUT"
                    });

                dto.Links.Add(
                    new HateoasData()
                    {
                        Link = url.Link("DeleteCategory", new { id = dto.Id }),
                        Description = "self",
                        Method = "DELETE"
                    });
            }
        }

        public async Task GenerateItemLinks(ItemDto dto)
        {
            var url = BuildUrlHelper();

            dto.Links.Add(
                new HateoasData()
                {
                    Link = url.Link("GetItem", new { id = dto.Id }),
                    Description = "self",
                    Method = "GET"
                });

            dto.Links.Add(
                    new HateoasData()
                    {
                        Link = url.Link("UpdateItem", new { id = dto.Id }),
                        Description = "self",
                        Method = "PUT"
                    });

            dto.Links.Add(
                new HateoasData()
                {
                    Link = url.Link("RemoveItem", new { id = dto.Id }),
                    Description = "self",
                    Method = "DELETE"
                });
        }

        public async Task GenerateItemLinks(IEnumerable<ItemDto> dtos)
        {
            var url = BuildUrlHelper();

            foreach (ItemDto dto in dtos)
            {
                dto.Links.Add(
                    new HateoasData()
                    {
                        Link = url.Link("GetItem", new { id = dto.Id }),
                        Description = "self",
                        Method = "GET"
                    });

                dto.Links.Add(
                        new HateoasData()
                        {
                            Link = url.Link("UpdateItem", new { id = dto.Id }),
                            Description = "self",
                            Method = "PUT"
                        });

                dto.Links.Add(
                    new HateoasData()
                    {
                        Link = url.Link("RemoveItem", new { id = dto.Id }),
                        Description = "self",
                        Method = "DELETE"
                    });
            }
        }

        private IUrlHelper BuildUrlHelper()
        {
            var factory = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            return factory.GetUrlHelper(_actionContextAccessor.ActionContext);
        }
    }
}
