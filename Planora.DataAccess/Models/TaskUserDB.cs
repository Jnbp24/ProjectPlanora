using System;
using System.Collections.Generic;
using System.Text;

namespace Planora.DataAccess.Models
{
    public class TaskUserDB
    {
        public Guid TaskId { get; set; }
        public TaskDB Task { get; set; }

        public Guid UserId { get; set; }
        public UserDB User { get; set; }
    }
}
