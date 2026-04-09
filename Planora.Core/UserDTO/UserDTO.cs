using System;
using System.Collections.Generic;
using System.Text;

namespace Planora.Core.DTO
{
    public record UserDTO(string Id, string FirstName, string LastName, string Email, bool Tovholder);
}