using System;
using System.Collections.Generic;
using System.Text;

namespace Planora.Core.DTO
{
    public class UserDTO
    {
		public Guid Id { get; set; }
		public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Tovholder { get; set; }

        public UserDTO() { }
    }
}