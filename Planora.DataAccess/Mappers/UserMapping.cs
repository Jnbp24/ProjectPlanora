using System;
using System.Collections.Generic;
using System.Text;
using Planora.Core.DTO;

namespace Planora.DataAccess.Mappers
{
    internal static class UserMapping
    {
		internal static UserDB ToEntity(UserDTO dto)
		{
			return new UserDB
			{
				Id = new Guid(),
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Email = dto.Email,
				Tovholder = dto.Tovholder
			};
		}

		internal static UserDTO ToDTO(UserDB entity)
		{
			return new UserDTO
			{
				Id = entity.Id.ToString(),
				FirstName = entity.FirstName,
				LastName = entity.LastName,
				Email = entity.Email,
				Tovholder = entity.Tovholder
			};
		}
	}
}
