using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoManager.Domain.Common.Interfaces;

namespace ToDoManager.Domain.Entities
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ICollection<ToDoManager.Domain.Entities.Task> UserTask { get; set; } 
    }
}
