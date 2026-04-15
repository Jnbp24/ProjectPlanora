using Planora.DataAccess.Models;
using Planora.DTO.UserDTO;

namespace Planora.DataAccess.Mappers;

public static class UserMapping
{
	public static UserDB ToEntity(UserDTO dto)
	{
		return new UserDB
		{
			UserId = Guid.NewGuid(),
			FirstName = dto.FirstName,
			LastName = dto.LastName,
			Email = dto.Email,
			Tovholder = dto.Tovholder
		};
	}

	public static UserDTO ToDTO(UserDB entity)
	{
		return new UserDTO(
			UserId: entity.UserId.ToString(),
			FirstName: entity.FirstName,
			LastName: entity.LastName,
			Email: entity.Email,
			Tovholder: entity.Tovholder
		);
	}
}