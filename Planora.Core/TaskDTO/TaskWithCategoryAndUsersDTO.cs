using Planora.DTO.Category;
using Planora.DTO.User;

namespace Planora.DTO.Task;

public record TaskWithCategoryAndUsersDTO(
	string? TaskId, 
	string Content, 
	string Title, 
	DateTime? Deadline, 
	string? CategoryId, 
	CategoryDTO? Category,
	List<UserDTO>? Users,
	string? CalenderYearId,
	bool Done
);