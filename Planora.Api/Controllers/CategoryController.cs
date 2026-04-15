using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planora.Api.Services.Category;
using Planora.DTO.CategoryDTO;

namespace Planora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase 
{
	private readonly ICategoryService _categoryService;
	
	public CategoryController(ICategoryService categoryService) 
	{
		_categoryService = categoryService;
	}
	
	// POST api/category
	[Authorize]
	[HttpPost]
	public async Task<ActionResult<CategoryDTO>> CreateCategoryAsync([FromBody] CategoryDTO categoryDTO)
	{
		var createdCategoryDto = await _categoryService.CreateAsync(categoryDTO);
		// return 201 with location header pointing to the created resource
		return CreatedAtAction(nameof(GetCategoryByIdAsync), new { categoryId = createdCategoryDto.CategoryId }, createdCategoryDto);
	}

	// GET api/category
	[Authorize]
	[HttpGet]
	public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync()
	{
		return Ok(await _categoryService.GetAllAsync());
	}
	
	// GET api/category/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
	[Authorize]
	[HttpGet("{categoryId}")]
	public async Task<ActionResult<CategoryDTO>> GetCategoryByIdAsync(string categoryId)
	{
		try
		{
			return Ok(await _categoryService.GetByIdAsync(categoryId));
		}
		catch (KeyNotFoundException e)
		{
			return NotFound(e.Message);
		}
	}
	
	// PUT api/category/d3eb20c6-2b60-4c82-95e3-
	[Authorize]
	[HttpPut("{categoryId}")]
	public async Task<IActionResult> UpdateCategoryAsync(string categoryId, CategoryDTO categoryDTO)
	{
		try
		{
			return Ok(await _categoryService.UpdateAsync(categoryId, categoryDTO));
		}
		catch (KeyNotFoundException e)
		{
			return NotFound(e.Message);
		}
	}
	
	// DELETE api/category/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
	[Authorize]
	[HttpDelete("{categoryId}")]
	public async Task<IActionResult> DeleteCategoryAsync(string categoryId)
	{
		try
		{
			await _categoryService.DeleteAsync(categoryId);
			return NoContent();
		}
		catch (KeyNotFoundException e)
		{
			return NotFound(e.Message);
		}
	}
}