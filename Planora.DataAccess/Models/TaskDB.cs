using System;
using System.Collections.Generic;
using System.Text;

namespace Planora.DataAccess.Models
{
    internal class TaskDB
    {
        public TaskDB()
        {
        }
        public Guid TaskID { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
