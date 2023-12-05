using CatalogService.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CatalogService.Filters
{
    public class HateoasCategoryFilterAttribute : ResultFilterAttribute
    {
        private readonly Services.LinkGenerator _linkGenerator;

        public HateoasCategoryFilterAttribute(Services.LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            var result = context.Result as ObjectResult;

            if (result.Value is CategoryDto)
            {
                var dto = result.Value as CategoryDto;
                await _linkGenerator.GenerateCategoryLinks(dto);
            }
            else if(result.Value is IEnumerable<CategoryDto>)
            {
                var dtos = result.Value as IEnumerable<CategoryDto>;
                await _linkGenerator.GenerateCategoryLinks(dtos);
            }
            
            await next();
        }
    }
}
