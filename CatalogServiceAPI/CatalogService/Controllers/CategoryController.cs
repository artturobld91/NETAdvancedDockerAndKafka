using CatalogService.Application.Dtos;
using CatalogService.Application.Interfaces;
using CatalogService.Application.Mappers;
using CatalogService.Application.Services;
using CatalogService.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger) 
        {
            _logger = logger;
            _categoryService = new CategoryService();
        }

        [HttpGet("GetCategories")]
        [Authorize(Policy = "Manager, Buyer")]
        [ServiceFilter(typeof(HateoasCategoryFilterAttribute))]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public IEnumerable<CategoryDto> GetCategories()
        {
            return _categoryService.GetCategories().ToDto();
        }

        [HttpGet("GetCategory/{id}", Name = "GetCategory")]
        [Authorize(Policy = "Manager, Buyer")]
        [ServiceFilter(typeof(HateoasCategoryFilterAttribute))]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        public ActionResult<CategoryDto> GetCategory([FromRoute] int id)
        {
            CategoryDto categoryDto = _categoryService.GetCategory(id).ToDto();
            return categoryDto is null ? NotFound() : Ok(categoryDto);
        }

        [HttpPost("AddCategory")]
        [Authorize(Policy = "Manager")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status201Created)]
        public IActionResult AddCategory([FromBody] CategoryCreateDto category)
        {
            _categoryService.AddCategory(category);
            return Ok();
        }

        [HttpDelete("DeleteCategory/{id}", Name = "DeleteCategory")]
        [Authorize(Policy = "Manager")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status204NoContent)]
        public IActionResult DeleteCategory([FromRoute] int id)
        {
            _categoryService.DeleteCategory(id);
            return NoContent();
        }

        [HttpPut("UpdateCategory/{id}", Name = "UpdateCategory")]
        [Authorize(Policy = "Manager")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status204NoContent)]
        public IActionResult UpdateCategory([FromRoute] int id, [FromBody] CategoryUpdateDto category)
        {
            category.Id = id;
            _categoryService.UpdateCategory(category);
            return NoContent();
        }
    }
}
