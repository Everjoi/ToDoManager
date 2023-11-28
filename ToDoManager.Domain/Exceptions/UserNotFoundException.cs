using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoManager.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string id) : base($"User (email: {id}) is not found")
        {
            
        }
    }
}
