using Planora.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Planora.DataAccess
{
    public class UserDB
    {
        public required Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
        public bool Tovholder { get; set; }
        public bool Deleted { get; set; }
        public UserDB() { }
    }
}