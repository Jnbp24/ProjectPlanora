using Microsoft.AspNetCore.Mvc;
using Planora.Api.Services;
using Planora.Api.Services.Category;
using Planora.DTO.CategoryDTO;

namespace Planora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase 
{
	private ICategoryService _categoryService;
	
	public CategoryController(ICategoryService categoryService) 
	{
		_categoryService = categoryService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllAsync()
	{
		var categories = await _categoryService.GetAllAsync();
		return Ok(categories);
	}
}