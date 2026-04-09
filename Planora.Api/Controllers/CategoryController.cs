using Microsoft.AspNetCore.Mvc;
using Planora.Api.Services;
using Planora.DTO.CategoryDTO;

namespace Planora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase {
	private CategoryService _categoryService;
	public CategoryController(CategoryService categoryService) {
		_categoryService = categoryService;
	}

	[HttpGet]
	public async Task<List<CategoryDTO>> GetCategories() {
		return await _categoryService.GetCategoriesAsync();
	}
}