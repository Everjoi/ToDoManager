using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoManager.Domain.Entities;

namespace ToDoManager.Application.DTOs
{
    public  class TaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsComleated { get; set; } = false;

    }
}
