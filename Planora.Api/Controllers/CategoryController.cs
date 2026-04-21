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
		var createdCategoryDto = await _categoryService.CreateCategoryAsync(categoryDTO);
		return CreatedAtAction(nameof(GetCategoryByIdAsync), new { categoryId = createdCategoryDto.CategoryId }, createdCategoryDto);
	}

	// GET api/category
	[Authorize]
	[HttpGet]
	public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync()
	{
		return Ok(await _categoryService.GetAllCategoriesAsync());
	}
	
	// GET api/category/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
	[Authorize]
	[HttpGet("{categoryId}")]
	public async Task<ActionResult<CategoryDTO>> GetCategoryByIdAsync(string categoryId)
	{
		return Ok(await _categoryService.GetCategoryByIdAsync(categoryId));
	}
	
	// PUT api/category/d3eb20c6-2b60-4c82-95e3-
	[Authorize]
	[HttpPut("{categoryId}")]
	public async Task<IActionResult> UpdateCategoryByIdAsync(string categoryId, [FromBody] CategoryDTO categoryDTO)
	{
		return Ok(await _categoryService.UpdateCategoryByIdAsync(categoryId, categoryDTO));
	}
	
	// DELETE api/category/d3eb20c6-2b60-4c82-95e3-b5be7f72cfdc
	[Authorize(Roles = "Tovholder")]
	[HttpDelete("{categoryId}")]
	public async Task<IActionResult> DeleteCategoryByIdAsync(string categoryId)
	{
		await _categoryService.DeleteCategoryByIdAsync(categoryId);
		return NoContent();
	}
}