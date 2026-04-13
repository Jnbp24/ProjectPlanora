using Microsoft.AspNetCore.Mvc;
using Planora.Api.Services;
using Planora.DTO.CategoryDTO;

namespace Planora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase 
{
	private readonly CategoryService _categoryService;
	
	public CategoryController(CategoryService categoryService) 
	{
		_categoryService = categoryService;
	}
	
	// POST api/task
	[HttpPost]
	public async Task<ActionResult<CategoryDTO>> CreateAsync([FromBody] CategoryDTO categoryDto)
	{
		var createdCategoryDto = await _categoryService.CreateAsync(categoryDto);
		// return 201 with location header pointing to the created resource
		return CreatedAtAction(nameof(GetCategoryByIdAsync), new { categoryId = createdCategoryDto.CategoryId }, createdCategoryDto);
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync()
	{
		return Ok(await _categoryService.GetAllAsync());
	}
	
	[HttpGet("{categoryId}")]
	public async Task<ActionResult<CategoryDTO>> GetCategoryByIdAsync(string categoryId)
	{
		try
		{
			return Ok(await _categoryService.GetByIdAsync(categoryId));
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
	}
	
	[HttpPut("{categoryId}")]
	public async Task<IActionResult> UpdateCategoryAsync(string categoryId, CategoryDTO categoryDto)
	{
		try
		{
			return Ok(await _categoryService.UpdateAsync(categoryId, categoryDto));
		}
		catch (KeyNotFoundException e)
		{
			return NotFound();
		}
	}


	
	[HttpDelete("{categoryId}")]
	public async Task<IActionResult> DeleteCategoryAsync(string categoryId)
	{
		try
		{
			return Ok(await _categoryService.DeleteAsync(categoryId));
		}
		catch (KeyNotFoundException e)
		{
			return NotFound();
		}
	}
}