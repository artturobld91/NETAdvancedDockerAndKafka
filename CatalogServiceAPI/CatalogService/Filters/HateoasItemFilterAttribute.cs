using CatalogService.Application.Dtos;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Filters
{
    public class HateoasItemFilterAttribute : ResultFilterAttribute
    {
        private readonly Services.LinkGenerator _linkGenerator;

        public HateoasItemFilterAttribute(Services.LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            var result = context.Result as ObjectResult;

            if (result.Value is ItemDto)
            {
                var dto = result.Value as ItemDto;
                await _linkGenerator.GenerateItemLinks(dto);
            }
            else if (result.Value is IEnumerable<ItemDto>)
            {
                var dtos = result.Value as IEnumerable<ItemDto>;
                await _linkGenerator.GenerateItemLinks(dtos);
            }

            await next();
        }
    }
}
