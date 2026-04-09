using System;
using System.Collections.Generic;
using System.Text;

namespace Planora.DataAccess
{
    internal class UserDB
    {
        public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
        public bool Tovholder { get; set; }
        public bool Deleted { get; set; }

        public UserDB() { }
    }
}